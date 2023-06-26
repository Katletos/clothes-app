namespace Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string SecretKey { get; set; }

    public double ExpiredTimeMinutes { get; set; }
}