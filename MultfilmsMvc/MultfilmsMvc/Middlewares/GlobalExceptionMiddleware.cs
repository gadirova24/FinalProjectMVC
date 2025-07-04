using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Exceptions;

namespace MultfilmsMvc.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // call the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                // Create ProblemDetails instance
                var problemDetails = new ProblemDetails
                {
                    Title = "An error occurred",
                    Detail = ex.Message
                };

                switch (ex)
                {
                    case ArgumentNullException argumentNullException:
                        // Handle ArgumentNullException
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Detail = argumentNullException.Message;
                        break;

                    case UnauthorizedAccessException unauthorizedAccessException:
                        // Handle UnauthorizedAccessException
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        problemDetails.Status = StatusCodes.Status401Unauthorized;
                        problemDetails.Detail = unauthorizedAccessException.Message;
                        break;

                    case InvalidOperationException invalidOperationException:
                        // Handle InvalidOperationException
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        problemDetails.Status = StatusCodes.Status409Conflict;
                        problemDetails.Detail = invalidOperationException.Message;
                        break;

                    case NotFoundException notFoundException:
                        // Handle custom NotFoundException
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        problemDetails.Status = StatusCodes.Status404NotFound;
                        problemDetails.Detail = notFoundException.Message;
                        break;
                    default:
                        // Handle general exceptions
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        problemDetails.Detail = "An unexpected error occurred.";
                        break;
                }

                // Write the response as JSON
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }


    }
}

