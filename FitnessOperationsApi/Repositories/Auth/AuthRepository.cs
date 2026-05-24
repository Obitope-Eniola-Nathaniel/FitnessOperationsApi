using BCrypt.Net;
using FitnessOperationsApi.Data;
using FitnessOperationsApi.DTOs.Auth;
using FitnessOperationsApi.Models;
using FitnessOperationsApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FitnessOperationsApi.Repositories.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    private readonly ITokenService _tokenService;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthRepository(AppDbContext context,ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _context = context;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<string> RegisterAsync( RegisterRequest request)
    {
        var exists = await _context.Users.AnyAsync(x => x.Email == request.Email);

        if (exists) throw new Exception("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash =BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return "User registered successfully";
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>x.Email == request.Email);

        if (user == null)throw new Exception("Invalid credentials");

        var validPassword =BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!validPassword)throw new Exception("Invalid credentials");

        /*
            Generate Tokens
        */

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        /*
            Save Refresh Token
        */

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        await _refreshTokenRepository.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(
        RefreshTokenRequest request)
    {
        /*
            Validate Existing Token
        */

        var existingToken =
            await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (existingToken == null)throw new Exception("Invalid refresh token");

        if (existingToken.IsExpired)throw new Exception("Refresh token expired");

        if (existingToken.IsRevoked)throw new Exception("Refresh token revoked");

        /*
            Generate New Tokens
        */

        var newAccessToken = _tokenService.GenerateAccessToken(existingToken.User);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        /*
            Revoke Old Token
        */

        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.ReplacedByToken = newRefreshToken;

        /*
            Create New Refresh Token
        */

        var refreshTokenEntity =
            new RefreshToken
            {
                Id = Guid.NewGuid(),

                UserId = existingToken.UserId,

                Token = newRefreshToken,

                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<string> LogoutAsync(
        RefreshTokenRequest request)
    {
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (existingToken == null)throw new Exception("Invalid refresh token");

        existingToken.RevokedAt = DateTime.UtcNow;

        await _refreshTokenRepository.SaveChangesAsync();

        return "Logged out successfully";
    }
}