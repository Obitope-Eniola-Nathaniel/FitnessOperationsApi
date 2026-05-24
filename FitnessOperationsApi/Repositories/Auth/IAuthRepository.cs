using FitnessOperationsApi.DTOs.Auth;

namespace FitnessOperationsApi.Repositories.Auth;

public interface IAuthRepository
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);

    Task<string> LogoutAsync(RefreshTokenRequest request);
}