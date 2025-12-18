using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.ComponentModel;


namespace PosApi.DTOs
{
    public class SuccessResponse<T,K>
    {
        [DefaultValue("Method Successfull")]
        [JsonPropertyName("message")]
        [Required]
        public string Message { get; set; }

        [DefaultValue(200)]
        [JsonPropertyName("status_code")]
        [Range(100, 599)]
        public int StatusCode { get; set; } = 200;

        [DefaultValue(null)]
        [JsonPropertyName("data")]
        public T Data { get; set; }
        private readonly ILogger<K> _logger;

        public SuccessResponse(T data, string message, int statusCode, ILogger<K> logger)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
            _logger = logger;
            _logger.LogInformation("SuccessResponse created with status code {StatusCode} and message: {Message}", statusCode, message);
        }
       
    }
}
