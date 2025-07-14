namespace ReferenceClientProxyImplementation.Patches;

public interface IPatch {
    public int GetOrder();
    public string GetName();
    public bool Applies(string relativeName, byte[] content);
    public Task<byte[]> Execute(string relativeName, byte[] content);
}