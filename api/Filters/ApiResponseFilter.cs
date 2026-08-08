using api.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Filters;

public class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var statusCode = objectResult.StatusCode ?? context.HttpContext.Response.StatusCode;
            var success = statusCode >= 200 && statusCode < 300;
            var message = success ? "Request processed successfully." : "An error occurred.";
            object? data = objectResult.Value;

            // Prevent double wrapping if the result is already ApiResponse
            if (data?.GetType().IsGenericType == true &&
                data.GetType().GetGenericTypeDefinition() == typeof(ApiResponse<>))
            {
                await next();
                return;
            }

            if (!success)
            {
                if (data is string errorMsg)
                {
                    message = errorMsg;
                }
                else if (data is ProblemDetails problem)
                {
                    message = problem.Title ?? problem.Detail ?? "An error occurred.";
                }
                else if (data != null)
                {
                    // Fallback for other error objects (e.g. SerializableError for model validation)
                    message = "An error occurred.";
                }
                
                data = null;
            }

            var apiResponse = new ApiResponse<object>(
                success,
                statusCode,
                message,
                data
            );

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = statusCode
            };
        }
        else if (context.Result is StatusCodeResult statusCodeResult)
        {
            var statusCode = statusCodeResult.StatusCode;
            var success = statusCode >= 200 && statusCode < 300;
            var message = success ? "Request processed successfully." : "An error occurred.";

            var apiResponse = new ApiResponse<object>(
                success,
                statusCode,
                message,
                null
            );

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = statusCode
            };
        }
        else if (context.Result is EmptyResult)
        {
            var apiResponse = new ApiResponse<object>(
                true,
                200,
                "Request processed successfully.",
                null
            );

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = 200
            };
        }

        await next();
    }
}
