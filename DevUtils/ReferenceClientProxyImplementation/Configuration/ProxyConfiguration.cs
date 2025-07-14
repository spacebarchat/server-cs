namespace ReferenceClientProxyImplementation.Configuration;

public class ProxyConfiguration {
    public ProxyConfiguration(IConfiguration configuration) => configuration.GetRequiredSection("ProxyConfiguration").Bind(this);

    public TestClientConfig TestClient { get; set; } = new();
    public AssetCacheConfig AssetCache { get; set; } = new();
}