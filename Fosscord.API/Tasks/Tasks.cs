using Fosscord.API.Tasks.Startup;

namespace Fosscord.API.Tasks;

public class Tasks
{
    public static void RunStartup()
    {
        BuildClientTask.Execute();
    }
}