namespace ReferenceClientProxyImplementation.Configuration;

public class ProxyConfiguration
{
    public ProxyConfiguration(IConfiguration configuration)
    {
        configuration.GetSection("ProxyConfiguration").Bind(this);
    }
    
    
}