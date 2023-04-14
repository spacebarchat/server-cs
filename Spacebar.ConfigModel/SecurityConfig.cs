using Spacebar.Static.Utilities;

namespace Spacebar.ConfigModel;

public class SecurityConfig
{
    public string JwtSecret { get; set; } = RandomStringGenerator.Generate(255);
    public string? IssuerSigningKey { get; set; } = null!;
    public RegisterSecurityConfig Register { get; set; } = new();
    public LoginSecurityConfig Login { get; set; } = new();
}