using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PosApi.DTOs;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using PosApi.Exceptions; // for AppValidationException
using PosApi.Shared; // for AppErrorCode
using System.Collections.Generic;

namespace PosApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
            catch (AppValidationException ex)
            {
                _logger.LogError(ex, "App validation exception");
                // Map AppValidationException -> ErrorResponse<IReadOnlyList<string>>
                var errorResponse = new ErrorResponse<IReadOnlyList<string>>(
                    errors: ex.Errors,
                    message: ex.Message,
                    statusCode: (int)(ex.Code ?? AppErrorCode.None),
                    logger: _logger
                );
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access exception occurred.");
                var errorResponse = new ErrorResponse<string>(
                    errors: ex.StackTrace, // or null if you don't want to expose stack trace
                    message: ex.Message,
                    statusCode: (int)AppErrorCode.UNAUTHORIZED_ACCESS,
                    logger: _logger
                );
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request exception occurred.");
                var errorResponse = new ErrorResponse<string>(
                    errors: httpEx.StackTrace,
                    message: httpEx.Message,
                    statusCode: (int)AppErrorCode.INTERNAL_SERVER_ERROR,
                    logger: _logger
                );
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogError(ex, "Token expired exception occurred.");
                var errorResponse = new ErrorResponse<string>(
                    errors: ex.StackTrace,
                    message: "Token has expired.",
                    statusCode:(int)AppErrorCode.TOKEN_EXPIRED,
                    logger: _logger
                );
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                _logger.LogError(ex, "Invalid token signature exception occurred.");
                var errorResponse = new ErrorResponse<string>(
                    errors: ex.StackTrace,
                    message: "Invalid token signature.",
                    statusCode:(int)AppErrorCode.UNAUTHENTICATED,
                    logger: _logger
                );
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Security token exception occurred.");
                var errorResponse = new ErrorResponse<string>(
                    errors: ex.StackTrace,
                    message: "Invalid token.",
                    statusCode:(int)AppErrorCode.UNAUTHORIZED_ACCESS,
                    logger: _logger
                );
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex) when (
                ex is KeyNotFoundException ||
                ex is ArgumentException ||
                ex is InvalidOperationException
            )
            {
                _logger.LogError(ex, "Resource or argument error.");
                var statusCode = ex is KeyNotFoundException
                    ? (int) AppErrorCode.NOT_FOUND
                    : (int) AppErrorCode.BAD_REQUEST;
                var status = ex is KeyNotFoundException
                    ? (int)HttpStatusCode.NotFound
                    : (int)HttpStatusCode.BadRequest;
                var errorResponse = new ErrorResponse<string>(
                    errors: ex.GetBaseException().Message,
                    message: ex.Message,
                    statusCode: statusCode,
                    logger: _logger
                );
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response started, skip error write: {Message}", ex.Message);
                    return;
                }
                context.Response.Clear();
                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                var errorResponse = new ErrorResponse<string>(
                    errors: ex.StackTrace,
                    message: "An unexpected error occurred.",
                    statusCode: (int)AppErrorCode.INTERNAL_SERVER_ERROR,
                    logger: _logger
                );
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response started, skip error write: {Message}", ex.Message);
                    return;
                }
                context.Response.Clear();
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
