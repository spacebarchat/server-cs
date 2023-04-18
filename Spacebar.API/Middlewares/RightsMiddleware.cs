using System.Text;
using Spacebar.Static.Attributes;
using Spacebar.Util;

namespace Spacebar.API.Middlewares;

public class RightsMiddleware
{
    private RequestDelegate _next;

    public RightsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var attribute = endpoint?.Metadata.GetMetadata<RequireRightsAttribute>();
        if (attribute != null)
        {
            if (context.Request.Headers["Authorization"].ToString() == "")
            {
                context.Response.StatusCode = 401;
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("This route requires authorization"));
                return;
            }

            var jwtam =
                context.RequestServices.GetService(typeof(JwtAuthenticationManager)) as JwtAuthenticationManager;
            var user = await jwtam.GetUserFromToken(context.Request.Headers["Authorization"].ToString().Replace("Bot ", ""));
            if (!attribute.HasRights(user.Rights))
                throw new UnauthorizedAccessException("You don't have the rights to do this.");
        }

        await _next(context);
    }
}