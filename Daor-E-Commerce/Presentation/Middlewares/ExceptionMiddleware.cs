//using System.Net;
//using Daor_E_Commerce.Common;

//public class ExceptionMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionMiddleware> _logger;   

//    public ExceptionMiddleware(
//        RequestDelegate next,
//        ILogger<ExceptionMiddleware> logger)                  
//    {
//        _next = next;
//        _logger = logger;                                     
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, ex.Message);                 

//            context.Response.StatusCode =
//                (int)HttpStatusCode.InternalServerError;     

//            context.Response.ContentType = "application/json";

//            var response = new ApiResponse<string>(
//                500,
//                "Something went wrong",
//                ex.Message
//            );

//            await context.Response.WriteAsJsonAsync(response);
//        }
//    }
//}



using Daor_E_Commerce.Common;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
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
