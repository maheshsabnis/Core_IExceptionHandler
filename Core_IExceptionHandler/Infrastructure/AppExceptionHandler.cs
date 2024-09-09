using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core_IExceptionHandler.Infrastructure
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is DbUpdateException)
            { 
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var errorDetails = new ErrorDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    TypeName = exception.GetType().Name,
                    ErrorTitle = "Database Error",
                    Message = exception.Message,
                    RequestDetails = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };

                httpContext.Response.WriteAsJsonAsync(errorDetails);
                return new ValueTask<bool>(true);
            }

            return new ValueTask<bool>(false);
        }
    }
}
