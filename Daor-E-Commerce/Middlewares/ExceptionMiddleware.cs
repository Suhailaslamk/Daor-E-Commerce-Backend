using System.Net;
using Daor_E_Commerce.Common;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<string>(
                500,
                "Something went wrong",
                ex.Message
            );

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
