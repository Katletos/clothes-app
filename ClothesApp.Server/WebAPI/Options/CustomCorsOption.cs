namespace WebAPI.Options;

public class CorsOptions
{
    public const string SectionName = "CorsPolicies";

    public string Name { get; set; }

    public string[] Origins { get; set; }
}