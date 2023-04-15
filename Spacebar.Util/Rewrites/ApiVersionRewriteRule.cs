using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace Spacebar.Util.Rewrites;

public class ApiVersionRewriteRule : IRule
{
    public void ApplyRule(RewriteContext context)
    {
        var request = context.HttpContext.Request;

        for (var i = 0; i < 32; i++)
            if (request.Path.Value.ToLower().StartsWith($"/api/v{i}/"))
            {
                request.Path = request.Path.Value.Replace($"/api/v{i}/", "/api/");
                context.Result = RuleResult.SkipRemainingRules;
                return;
            }
    }
}