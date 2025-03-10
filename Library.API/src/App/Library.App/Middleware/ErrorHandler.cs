using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.App.Middleware
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                switch (context.Response.StatusCode)
                {
                    case 400:
                        await HandleValidationErrors(context); // New method for validation errors
                        break;
                    case 401:
                        await HandleException(context, 401, "Unauthorized please login first");
                        break;

                    case 403:
                        await HandleException(context, 403, "Not allowed to access here", true);
                        break;

                    case 404:
                        await HandleException(context, 404, "Requested url not found");
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                await HandleException(context, 500, "something went wrong please contact developer");
            }
            finally
            {
            }
        }

        private async static Task HandleException(HttpContext context, int statusCode, string errorMessage, bool isForbidden = false)
        {
            if (!context.Response.HasStarted)
            {
                var result = isForbidden ? JsonConvert.SerializeObject(new { result = new List<object>() })
            : JsonConvert.SerializeObject(new { message = errorMessage });
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        }

        private async Task HandleValidationErrors(HttpContext context)
        {
            var validationErrors = context.Features.Get<ValidationProblemDetails>();
            var errors = validationErrors?.Errors
                .SelectMany(e => e.Value)
                .ToList();

            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = errors
            });

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }
}