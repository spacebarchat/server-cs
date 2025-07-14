namespace ReferenceClientProxyImplementation.Configuration;

public class TestClientConfig {
    public TestClientDebug DebugOptions = new();
    public bool Enabled { get; set; } = true;
    // public bool UseLatest = true;
    public string Revision { get; set; } = "canary";
    public Dictionary<string, object> GlobalEnv { get; set; } = new();
    
    // internal
    public string RevisionPath { get; set; } = null!;
    public string RevisionBaseUrl { get; set; } = null!;
}