using System.Net;
using Fosscord.API.Utilities;
using Fosscord.DbModel;

namespace Fosscord.API.Tasks.Startup;

public class BuildClientTask
{
    public static void Execute()
    {
        if (!FosscordConfig.GetBool("client_useTestClient", true) ||
            !FosscordConfig.GetBool("client_testClient_latest", false)) return;
        Console.WriteLine("[Client updater] Fetching client");
        string client = HtmlUtils.CleanupHtml(new WebClient().DownloadString("https://discord.com/channels/@me"));
        
    }
}

