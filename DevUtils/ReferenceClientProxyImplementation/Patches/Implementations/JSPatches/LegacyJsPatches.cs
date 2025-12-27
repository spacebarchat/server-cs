// using System.Text;
// using System.Text.RegularExpressions;
//
// namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;
//
// public partial class LegacyJsPathces : IPatch {
//     public int GetOrder() => 1;
//
//     public string GetName() => "Patch deprecated JS constructs";
//     public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");
//
//     public async Task<byte[]> Execute(string relativePath, byte[] content) {
//         var stringContent = Encoding.UTF8.GetString(content);
//
//         while(MozInputSourceRegex().IsMatch(stringContent)) {
//             var match = MozInputSourceRegex().Match(stringContent);
//             var replacement = match.Groups[1].Value switch {
//                 "0" => "",
//                 "1" => "mouse",
//                 "2" => "pen",
//                 "3" => "pen",
//                 "4" => "touch",
//                 _ => throw new InvalidOperationException("Unreachable")
//             };
//         }
//
//         return Encoding.UTF8.GetBytes(stringContent);
//     }
//
//     [GeneratedRegex(@"([0-6]) === (\w).mozInputSource", RegexOptions.Compiled)]
//     private static partial Regex MozInputSourceRegex();
// }