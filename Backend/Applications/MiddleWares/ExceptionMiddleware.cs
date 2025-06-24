

namespace InsurenceManagementSystemWebApi.Applications.MiddleWares;


public class ExceptionMiddleware

{

    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)

    {

        _next = next;

        _logger = logger;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        try

        {

            await _next(context);

        }

        catch (Exception ex)

        {

            _logger.LogError(ex, "An error occurred.");

            await HandleExceptionAsync(context);

        }

    }

    private static Task HandleExceptionAsync(HttpContext context)

    {

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var message = "An error occurred while processing your request.";

        return context.Response.WriteAsync(JsonConvert.SerializeObject(message));

    }

}


