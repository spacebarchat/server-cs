using System.Text;
using System.Text.RegularExpressions;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class DisableSciencePatch : IPatch {
    public int GetOrder() => 0;

    public string GetName() => @"JS(web): Disable /science calls";
    public bool Applies(string relativeName, byte[] content) => relativeName.StartsWith("assets/web.") && relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);
        
        var match = HandleTrackDefinitionRegex().Match(stringContent);
        stringContent = stringContent.Insert(match.Index + match.Length, @"
             return (new Promise(() => { }), false); // ReferenceClientProxyImplementation: Disable /science calls
        ");

        return Encoding.UTF8.GetBytes(stringContent);
    }
    
    [GeneratedRegex(@".\.handleTrack = function \(.\) \{", RegexOptions.Compiled)]
    private static partial Regex HandleTrackDefinitionRegex();
    
}