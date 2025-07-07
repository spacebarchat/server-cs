namespace ReferenceClientProxyImplementation.Tasks;

public class Tasks(IServiceProvider serviceProvider) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        var defaultColor = Console.ForegroundColor;
        var tasks = serviceProvider.GetServices<ITask>().ToList();
        // List<ITask> tasks = new()
        // {
        //     new BuildClientTask(),
        //     new PatchClientAssetsTask()
        // };
        var i = 0;
        foreach (var task in tasks.OrderBy(x => x.GetOrder())) {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("==> ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Running task {++i}/{tasks.Count}: {task.GetName()} (Type<{task.GetType().Name}>)");
            Console.ForegroundColor = defaultColor;
            task.Execute();
        }
    }
}

public interface ITask {
    public int GetOrder();
    public string GetName();
    public Task Execute();
}