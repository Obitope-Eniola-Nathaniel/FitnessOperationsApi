using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Repositories.Auth;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task SaveChangesAsync();
}
