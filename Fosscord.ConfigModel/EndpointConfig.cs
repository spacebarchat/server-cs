namespace Fosscord.ConfigModel;

public class EndpointConfig
{
    public string InternalGateway { get; set; } = "http://localhost:1999";
    public string Api { get; set; } = "http://localhost:2000";
    public string Gateway { get; set; } = "http://localhost:2001";
    public string Cdn { get; set; } = "http://localhost:2002";

    public string GetApiInternal()
    {
        return Api;
    }
    public string GetGatewayInternal()
    {
        return Gateway;
    }
    public string GetCdnInternal()
    {
        return Cdn;
    }
}
