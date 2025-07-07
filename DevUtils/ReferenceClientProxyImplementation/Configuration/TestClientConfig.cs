namespace ReferenceClientProxyImplementation.Configuration;

public class TestClientConfig {
    public TestClientDebug DebugOptions = new();
    public bool Enabled { get; set; } = true;
    // public bool UseLatest = true;
    public string Revision { get; set; } = "canary";
    public string RevisionPath { get; set; } = null!;
}