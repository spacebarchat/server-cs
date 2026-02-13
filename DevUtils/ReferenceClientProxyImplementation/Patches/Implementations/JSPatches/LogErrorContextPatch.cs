// using System.Text;
// using System.Text.RegularExpressions;
//
// namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;
//
// public partial class LogErrorContextPatch : IPatch {
//     public int GetOrder() => 2;
//
//     public string GetName() => "Patch assertions to log more context";
//     public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");
//
//     public async Task<byte[]> Execute(string relativePath, byte[] content) {
//         var stringContent = Encoding.UTF8.GetString(content);
//
//         stringContent = NotNullAssertionRegex().Replace(
//             stringContent,
//             m => {
//                 var methodName = m.Groups[1].Value;
//                 var objectName = m.Groups[2].Value;
//                 var message = m.Groups[3].Value;
//                 Console.WriteLine($"Patching not-null assertion in {relativePath}: {methodName} - {message}");
//
//                 return $@"{methodName}()(null != {objectName}, ""{message} - Context: "" + JSON.stringify({objectName}))";
//             }
//         );
//
//         return Encoding.UTF8.GetBytes(stringContent);
//     }
//     
//     // null assertion: u()(null != o, "PrivateChannel.renderAvatar: Invalid prop configuration - no user or channel");
//     // capture: method name, object name, message
//     [GeneratedRegex(@"([a-zA-Z0-9_]+)\(\)\(\s*null != ([a-zA-Z0-9_]+),\s*""([^""]+)""\s*\)", RegexOptions.Compiled)]
//     private static partial Regex NotNullAssertionRegex();
// }