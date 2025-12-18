namespace PosApi.DTOs
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;

    public class ErrorResponse<T>
    {
        [DefaultValue("Error")]
        [JsonPropertyName("message")]
        [Required]
        public string Message { get; set; } = string.Empty;

        [DefaultValue(1018)]
        [JsonPropertyName("status_code")]
        [Range(100, 599)]
        public int StatusCode { get; set; } = 1018;

        [JsonPropertyName("errors")]
        public T? Errors { get; set; }

        // Constructor không log
        public ErrorResponse(T? errors, string message, int statusCode = 1004)
        {
            Errors = errors;
            Message = message;
            StatusCode = statusCode;
        }

        // Constructor có log
        public ErrorResponse(T? errors, string message, int statusCode, ILogger logger)
        {
            Errors = errors;
            Message = message;
            StatusCode = statusCode;
            logger.LogError("ErrorResponse created with status code {StatusCode} and message: {Message}", statusCode, message);
        }

        // Static factory method (không log)
        public static ErrorResponse<T> Create(T? errors, string message = "Error", int statusCode = 1001)
        {
            return new ErrorResponse<T>(errors, message, statusCode);
        }
    }
}
