namespace Spacebar.API.Middlewares;

// Modified from https://stackoverflow.com/a/42199758
public class OptionsMiddleware(RequestDelegate next) {
    public Task Invoke(HttpContext context) => BeginInvoke(context);

    private Task BeginInvoke(HttpContext context) {
        context.Response.Headers.Add("Access-Control-Allow-Origin",
            new[] { (string)context.Request.Headers["Origin"] });
        context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "*" });
        context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
        if (context.Request.Method == "OPTIONS") {
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("OK");
        }

        return next.Invoke(context);
    }
}

public static class OptionsMiddlewareExtensions {
    public static IApplicationBuilder UseOptions(this IApplicationBuilder builder) => builder.UseMiddleware<OptionsMiddleware>();
}