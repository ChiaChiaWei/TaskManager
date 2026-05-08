using System.Net;
using System.Text.Json;
using TaskManager.API.Models;
using TaskManager.Core.Exceptions;

namespace TaskManager.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger) 
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
        catch(NotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            await HandleExceptionAsync(
                context,
                HttpStatusCode.NotFound, 
                ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex.Message);
            await HandleExceptionAsync(
                context, 
                HttpStatusCode.BadRequest, 
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            await HandleExceptionAsync(
                context, 
                HttpStatusCode.InternalServerError, 
                "An unexpected error occured");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context, 
        HttpStatusCode statusCode, 
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Status = (int)statusCode,
            Message = message
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy=JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
