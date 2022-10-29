using Fosscord.Static.Utilities;

namespace Fosscord.ConfigModel;

public class SecurityConfig
{
    public string JwtSecret { get; set; } = RandomStringGenerator.Generate(255);
    public string? IssuerSigningKey { get; set; } = null!;
    public RegisterSecurityConfig Register { get; } = new();
    public LoginSecurityConfig Login { get; set; } = new();
}