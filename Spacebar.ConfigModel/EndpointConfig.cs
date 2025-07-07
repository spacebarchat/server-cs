namespace Spacebar.ConfigModel;

public class EndpointConfig {
    public string InternalGateway { get; set; } = "http://localhost:1999";
    public string Api { get; set; } = "http://localhost:2000";
    public string Gateway { get; set; } = "http://localhost:2001";
    public string Cdn { get; set; } = "http://localhost:2002";

    public string GetApiInternal() => Api;

    public string GetGatewayInternal() => Gateway;

    public string GetCdnInternal() => Cdn;
}