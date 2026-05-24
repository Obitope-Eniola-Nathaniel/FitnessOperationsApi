namespace FitnessOperationsApi.Configuration;

public class JwtSettings
{
    public string Key { get; set; } = default!;

    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;

    public int AccessTokenExpiryMinutes { get; set; }

    public int RefreshTokenExpiryDays { get; set; }
}