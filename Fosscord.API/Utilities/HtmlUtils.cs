namespace Fosscord.API.Utilities;

public class HtmlUtils
{
    public static string CleanupHtml(string input)
    {
        input = input.Replace("<", "\n<");
        while (input.Contains("  ")) input = input.Replace("  ", " ");
        while (input.Contains("\n\n")) input = input.Replace("\n\n", "\n");
        while (input.Contains("\n \n")) input = input.Replace("\n \n", "\n");
        input = input.Replace(">\n</script>", "/>");
        input = input.Replace(">\n</link>", "/>");
        var lines = input.Split('\n');
        int indent = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("<")) indent++;
            if (lines[i].Contains(">")) indent--;
            lines[i] = new String(' ', indent*2)+lines[i];
        }

        input = string.Join('\n', lines);
        // File.WriteAllLines("cache/ind.html", lines);
        return input;
    }
}