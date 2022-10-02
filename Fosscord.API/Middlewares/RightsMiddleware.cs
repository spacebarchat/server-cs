using Fosscord.API.Classes;
using Fosscord.Shared.Attributes;
using Fosscord.Shared.Enums;
using Microsoft.AspNetCore.Http.Features;

namespace Fosscord.API.Middlewares;

public class RightsMiddleware
{
    private RequestDelegate _next;

    public RightsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // a try/catch can be added here because you don't want middleware failures to interfere with normal functionality
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<RequireRightsAttribute>();
        if (attribute != null)
        {
            var jwtam = context.RequestServices.GetService(typeof(JWTAuthenticationManager)) as JWTAuthenticationManager;
            var user = jwtam.GetUserFromToken(context.Request.Headers["Authorization"].ToString().Replace("Bot ", ""));
            if (!attribute.HasRights((Rights) user.Rights)) throw new UnauthorizedAccessException("You don't have the rights to do this.");
        }

        await _next(context); // Here the action in the controller is called
    }
}