using FluentValidation;
using System.Net;
using System.Text.Json;

namespace ProductManagement.Api.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles ValidationException thrown by the ValidationBehavior.
    /// Converts FluentValidation errors into a structured JSON response
    /// with proper HTTP 400 Bad Request status code.
    /// </summary>
    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        // Convert FluentValidation errors to a dictionary format
        // Groups errors by property name and collects all error messages for each property
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => ToCamelCase(g.Key), // Convert property names to camelCase for JSON
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        // Create a structured error response
        var response = new
        {
            statusCode = HttpStatusCode.BadRequest,
            message = "Validation failed",
            errors = errors,
            data = (object?)null
        };

        // Serialize with camelCase naming policy to match your API conventions
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    /// <summary>
    /// Converts PascalCase property names to camelCase for JSON serialization.
    /// Example: "CategoryName" ? "categoryName"
    /// </summary>
    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
            return str;

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }
}