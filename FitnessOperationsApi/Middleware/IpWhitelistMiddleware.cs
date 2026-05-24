namespace FitnessOperationsApi.Middleware;

public class IpWhitelistMiddleware
{
    private readonly RequestDelegate _next;

    private readonly string[] _allowedIps =
    {
        "127.0.0.1"
    };

    public IpWhitelistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (!_allowedIps.Contains(ip))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("IP not allowed");
            return;
        }

        await _next(context);
    }
}
