using System.Text;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public class ApiProtocolPatch : IPatch {
    public int GetOrder() => 0;

    public string GetName() => "API: Use GLOBAL_ENV.API_PROTOCOL instead of hardcoded https:";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        // TODO: regex
        stringContent = stringContent
                .Replace(
                    "return \"https:\" + window.GLOBAL_ENV.API_ENDPOINT + (e ? \"/v\".concat(window.GLOBAL_ENV.API_VERSION) : \"\");",
                    "return window.GLOBAL_ENV.API_PROTOCOL + window.GLOBAL_ENV.API_ENDPOINT + (e ? \"/v\".concat(window.GLOBAL_ENV.API_VERSION) : \"\");"
                )
                .Replace(
                    "api_endpoint: \"\".concat(\"https:\").concat(window.GLOBAL_ENV.API_ENDPOINT)",
                    "api_endpoint: window.GLOBAL_ENV.API_PROTOCOL.concat(window.GLOBAL_ENV.API_ENDPOINT)"
                )
                .Replace(
                    "f = null != d ? \"https://\".concat(d) : location.protocol + window.GLOBAL_ENV.API_ENDPOINT,",
                    "f = null != d ? window.GLOBAL_ENV.API_PROTOCOL.concat(d) : location.protocol + window.GLOBAL_ENV.API_ENDPOINT,"
                )
            ;

        return Encoding.UTF8.GetBytes(stringContent);
    }
}