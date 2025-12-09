namespace Million.RealEstate.Backend.Api.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiExceptionMiddleware>();
    }
}
