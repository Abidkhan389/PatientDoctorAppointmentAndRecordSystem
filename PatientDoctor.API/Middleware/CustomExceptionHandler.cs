using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Contracts.Persistance.IException;
using PatientDoctor.Application.Helpers.General.Exceptions;

namespace PatientDoctor.API.Middleware;

public sealed class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly IExceptionLogger _exceptionLogger;
    private readonly IWebHostEnvironment _env;

    public CustomExceptionHandler(
        ILogger<CustomExceptionHandler> logger,
        IExceptionLogger exceptionLogger,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _exceptionLogger = exceptionLogger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Ignore client disconnected
        if (exception is OperationCanceledException)
            return false;

        var (statusCode, title, detail, errors) = MapException(exception);

        // Log to console / file / app insights
        _logger.LogError(exception,
            "Unhandled exception occurred. TraceId: {TraceId}",
            context.TraceIdentifier);

        // Log to database
        await _exceptionLogger.LogAsync(
            context,
            exception,
            statusCode);

        // Decide message for client (dev vs prod)
        var clientDetail = _env.IsDevelopment()
            ? detail
            : (statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : detail);

        // Build ProblemDetails response
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = clientDetail,
            Instance = context.Request.Path
        };

        if (errors is not null)
            problemDetails.Extensions["errors"] = errors;

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static (int StatusCode, string Title, string Detail, object? Errors) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException ex => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation errors occurred.",
                ex.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
            ),

            BadRequestException ex => (
                StatusCodes.Status400BadRequest,
                ex.GetType().Name,
                ex.Message,
                null
            ),

            NotFoundException ex => (
                StatusCodes.Status404NotFound,
                ex.GetType().Name,
                ex.Message,
                null
            ),

            InternalServerException ex => (
                StatusCodes.Status500InternalServerError,
                ex.GetType().Name,
                ex.Message,
                null
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                exception.GetType().Name,
                exception.Message,
                null
            )
        };
    }
}
