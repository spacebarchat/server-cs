using System.Net;
using Fosscord.API.Utilities;

namespace Fosscord.API.Tasks.Startup;

public class BuildClientTask : ITask
{
    public string GetName()
    {
        return "Build updated test client";
    }

    public void Execute()
    {
        if (!Static.Config.TestClient.Enabled ||
            !Static.Config.TestClient.UseLatest) return;
        Console.WriteLine("[Client updater] Fetching client");
        string client = HtmlUtils.CleanupHtml(new WebClient().DownloadString("https://canary.discord.com/channels/@me"));
        Console.WriteLine("[Client updater] Building client...");
        string target = File.ReadAllText("Resources/Pages/index-template.html");
        var lines = client.Split("\n");
        target = target.Replace("<!--prefetch_script-->",
            string.Join("\n", lines.Where(x => x.Contains("link rel=\"prefetch\" as=\"script\""))));
        target = target.Replace("<!--client_script-->",
            string.Join("\n", lines.Where(x => x.Contains("<script src="))));
        target = target.Replace("<!--client_css-->",
            string.Join("\n", lines.Where(x => x.Contains("link rel=\"stylesheet\""))));
        target = target.Replace("integrity", "hashes");
        File.WriteAllText("Resources/Pages/index-updated.html", target);
        Console.WriteLine("[Client updater] Finished building client!");
    }
}