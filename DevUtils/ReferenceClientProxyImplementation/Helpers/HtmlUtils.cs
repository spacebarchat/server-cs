using AngleSharp.Html;
using AngleSharp.Html.Parser;

namespace ReferenceClientProxyImplementation.Helpers;

public class HtmlUtils {
    public static string CleanupHtml(string input) {
        var parser = new HtmlParser();

        var document = parser.ParseDocument(input);

        var sw = new StringWriter();
        document.ToHtml(sw, new PrettyMarkupFormatter());
        return sw.ToString();
    }
}