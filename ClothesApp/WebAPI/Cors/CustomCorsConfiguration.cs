namespace WebAPI.Cors;

public class CustomCorsConfiguration
{
    public const string Name = "DefaultCorsePolicy";

    public readonly static string[] Origins =
    {
        "http://127.0.0.1:5500",
        "http://127.0.0.1:5500",
        "https://127.0.0.1:5500",
        "https://127.0.0.1:5500",
    };
}