// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var stringContent =
    File.ReadAllText(
        "/home/Rory/git/spacebar/server-cs/DevUtils/ReferenceClientProxyImplementation/clientRepository/buildId_5c6864c96c1ce7c66084565164c03bacc098733d/patched/assets/660d52c8df3e3723.js");
var regex = new Regex(@"JSON\.parse\(\s*'(.*?)',?\s*\);", RegexOptions.Compiled | RegexOptions.Singleline);

var matches = regex.Matches(stringContent);

Console.WriteLine($"Found {matches.Count} JSON.parse calls");
foreach (Match match in matches) {
    // Extract the JSON string from the match
    await File.WriteAllTextAsync($"/tmp/dcjp_{Guid.NewGuid().ToString()}.json", match.Groups[1].Value);
}

RegexOptions options = RegexOptions.Multiline;

foreach (Match m in Regex.Matches(stringContent, regex.ToString(), options)) {
    Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
}