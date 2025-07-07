using Microsoft.Extensions.Configuration;
using Spacebar.ConfigModel.Gateway;
using Spacebar.DbModel;

namespace Spacebar.ConfigModel;

public class Config {
    public Config(IConfiguration config) => config.GetSection("Spacebar").Bind(this);

    public DbConfig DbConfig { get; set; } = new();
    public SentryConfig Sentry { get; set; } = new();
    public SecurityConfig Security { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
    public GatewayConfig Gateway { get; set; } = new();
    public EndpointConfig Endpoints { get; set; } = new();
}