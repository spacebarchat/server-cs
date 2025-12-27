using System.Text;
using System.Text.RegularExpressions;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Patches.Implementations.HTMLPatches;

public partial class StripNoncesPatch(ProxyConfiguration config) : IPatch {
    public int GetOrder() => 0;

    public string GetName() => "Strip nonces/integrity from html";
    public bool Applies(string relativeName, byte[] content) => relativeName is "app.html" or "developers.html";

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);
        stringContent = HtmlScriptIntegrityRegex().Replace(
            HtmlScriptNonceRegex().Replace(
                JsElementNonceRegex().Replace(
                    stringContent,
                    ""
                ),
                ""
            ),
            ""
        );
        return Encoding.UTF8.GetBytes(stringContent);
    }

    [GeneratedRegex("\\snonce=\"[a-zA-Z0-9+/=]+\"")]
    private static partial Regex HtmlScriptNonceRegex();

    [GeneratedRegex("\\w.nonce='[a-zA-Z0-9+/=]+';")]
    private static partial Regex JsElementNonceRegex();

    [GeneratedRegex(@"\sintegrity=""[a-zA-Z0-9+/=\-\s]+""")]
    private static partial Regex HtmlScriptIntegrityRegex();
}