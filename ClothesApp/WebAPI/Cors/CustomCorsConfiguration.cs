namespace WebAPI.Cors;

public class CustomCorsConfiguration
{
    public const string Name = "DefaultCorsePolicy";

    public readonly static string[] Origins =
    {
        "http://localhost:5500",
        "http://localhost:5500",
        "http://localhost:3000",
        "https://localhost:3000",
    };
}