namespace ReferenceClientProxyImplementation.Services;

public class TemporaryTestJob(BuildDownloadService buildDownloadService) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        Console.WriteLine("Running test job");
        var outDir =
            "/home/Rory/git/spacebar/server-cs/DevUtils/ReferenceClientProxyImplementation/downloadCache/today/raw/";
        if (Directory.Exists(outDir))
            Directory.Delete(outDir, true);
        Directory.CreateDirectory(outDir);
        // await buildDownloadService.DownloadBuildFromArchiveOrg(outDir, new DateTime(2014, 1, 1));
        await buildDownloadService.DownloadBuildFromUrl(outDir, "https://canary.discord.com/app");
    }
}