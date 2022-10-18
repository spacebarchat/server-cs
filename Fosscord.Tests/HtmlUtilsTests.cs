using Fosscord.Util.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Fosscord.Tests;

public class HtmlUtilsTests
{
    private readonly ITestOutputHelper output;

    public HtmlUtilsTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void FixIndentationTest()
    {
        var input = "<html>\n<div>\ntest\n</div>\n<html>";
        var expected = "<html>\n\t<head></head>\n\t<body>\n\t\t<div>\n\t\t\ttest\n\t\t</div>\n\t</body>\n</html>";
        var actual = HtmlUtils.CleanupHtml(input);
        Assert.Equal(expected, actual);
    }
}