using System.Globalization;
namespace ScholarshipFinderAPI.Middlewares;

public class HelloWorld
{
    private readonly RequestDelegate _next;
    public HelloWorld(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("Hello world");
        await _next(context);
    }
}
public static class HelloWorldMiddlewareExtensions
{
    public static IApplicationBuilder UseHelloWorld(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HelloWorld>();
    }
}