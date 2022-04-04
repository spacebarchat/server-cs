using ArcaneLibs;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Fosscord.Util.Formatters;

public class JsFormatter : Formatter
{
    public static void FormatJsFile(FileStream input, FileStream output)
    {
        GetLog().Log($"Formatting {input.Name} -> {output.Name}...");
        string inp = input.Name, outp = output.Name;
        SplitBySemicolons(input,output);
        File.Move(outp, outp+".tmp", true);
        FixIndent(File.OpenRead(outp+".tmp"), File.OpenWrite(outp));
        File.Delete(outp+".tmp");
    }

    public static void SplitBySemicolons(FileStream input, FileStream output)
    {
        var sr = new StreamReader(input);
        var sw = new StreamWriter(output);
        while (!sr.EndOfStream)
        {
            var curr = (char) sr.Read();
            sw.Write(curr);
            if(curr == ';') sw.Write('\n');
        }
        input.Close();
        input.Dispose();
        output.Flush();
        output.Close();
        output.Dispose();
    }
    public static void FixIndent(FileStream input, FileStream output)
    {
        var sr = new StreamReader(input);
        var sw = new StreamWriter(output);
        int indent = 0;
        while (!sr.EndOfStream)
        {
            var curr = sr.ReadLine().Trim();
            if (curr == "") continue;
            indent += curr.Count(x=>x=='{');
            indent -= curr.Count(x=>x=='}');
            if (indent < 0) indent = 0;
            sw.WriteLine(new string(' ', indent*4)+curr);
            // if(curr == ';') sw.Write('\n');
        }
        input.Close();
        input.Dispose();
        output.Flush();
        output.Close();
        output.Dispose();
    }
}