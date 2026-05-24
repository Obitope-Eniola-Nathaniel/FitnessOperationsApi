using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
