namespace ReferenceClientProxyImplementation.Patches.Implementations;

public class PatchSet(IServiceProvider sp) {
    public List<IPatch> Patches { get; } = sp.GetServices<IPatch>().OrderBy(x => x.GetOrder()).ToList();

    public async Task<byte[]> ApplyPatches(string relativeName, byte[] content) {
        var i = 0;
        var patches = Patches
            .Where(p => p.Applies(relativeName, content))
            .OrderBy(p => p.GetOrder())
            .ToList();
        foreach (var patch in patches) {
            if (patch.Applies(relativeName, content)) {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("==> ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Running task {++i}/{patches.Count}: {patch.GetName()} (Type<{patch.GetType().Name}>)");
                Console.ForegroundColor = defaultColor;
                content = await patch.Execute(relativeName, content);
            }
        }

        return content;
    }
}