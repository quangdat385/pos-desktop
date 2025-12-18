namespace PosApi.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Text.Json.Serialization;
    using PosApi.Entities;
    public class OrderDto
    {
        [JsonPropertyName("order_number")]
        [DefaultValue(null)]
        public string? OrderNumber { get; set; } = null;
        [JsonPropertyName("total_amount")]
        [DefaultValue(0.0)]
        [Required]
        public double TotalAmount
        {
            get; set;
        }
        [JsonPropertyName("items")]
        [Required]
        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
