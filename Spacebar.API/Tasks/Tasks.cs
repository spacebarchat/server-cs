namespace Spacebar.API.Tasks;

public class Tasks
{
    public static void RunStartup()
    {
        var defaultColor = Console.ForegroundColor;
        List<ITask> tasks = new()
        {
        };
        foreach (var task in tasks)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("==> ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(
                $"Running task {tasks.IndexOf(task) + 1}/{tasks.Count}: {task.GetName()} (Type<{task.GetType().Name}>)");
            Console.ForegroundColor = defaultColor;
            task.Execute();
        }
    }
}

public interface ITask
{
    public string GetName();
    public void Execute();
}