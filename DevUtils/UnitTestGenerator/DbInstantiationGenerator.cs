using System.Reflection;
using ArcaneLibs.Extensions;
using Fosscord.ConfigModel;
using Fosscord.DbModel;

namespace UnitTestGenerator;

public class DbInstantiationGenerator
{
    private static readonly string OutputPath = "../../../../../Fosscord.Tests/GeneratedTests/DbInstantiationTests.cs";

    public static void GenerateTests()
    {
        Console.WriteLine("Generating tests for instantiating all database models...");
        var type = typeof(Db);
        var methods = type.GetMethods().Where(x => x.IsStatic && x.ReturnType == typeof(Db)).ToList();
        var template = File.ReadAllText("Templates/DbInstantiation.txt");
        if (File.Exists(OutputPath))
            File.Delete(OutputPath);
        foreach (var methodInfo in methods)
        {
            if (methodInfo.Name == "GetDb") continue;
            //if (methodInfo.Name.ContainsAnyOf(new[] {"MySQL", "Postgres"})) continue;
            File.AppendAllText(OutputPath, template.Replace("$NAME", methodInfo.Name));
        }

        File.WriteAllText(OutputPath,
            File.ReadAllText("Templates/TestClass.txt")
                .Replace("$NAME", "DbInstantiationTests")
                .Replace("$CONTENT", File.ReadAllText(OutputPath))
                .Replace("//IMPORTS", "using Fosscord.DbModel;\nusing System;\nusing System.Linq;")
        );
    }
}