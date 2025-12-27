// using System.Text;
// using System.Text.RegularExpressions;
//
// namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;
//
// public partial class BooleanPropagationPatch : IPatch {
//     public int GetOrder() => 3;
//
//     public string GetName() => "Patch pointless boolean comparisons in JS";
//     public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");
//
//     public async Task<byte[]> Execute(string relativePath, byte[] content) {
//         var stringContent = Encoding.UTF8.GetString(content);
//
//         stringContent = stringContent
//             .Replace(" && true", "").Replace(" || false", "").Replace("false || ", "")
//             ;
//
//         return Encoding.UTF8.GetBytes(stringContent);
//     }
// }