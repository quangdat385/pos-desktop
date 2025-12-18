namespace PosApi.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Text.Json.Serialization;

    public class OrderResponseDto
    {
        [JsonPropertyName("id")]
        [DefaultValue(1)]
        public int Id { get; set; }
        
        [JsonPropertyName("order_number")]
        [DefaultValue("ORD-123456")]
        public string OrderNumber { get; set; } = string.Empty;
  
        [JsonPropertyName("total_amount")]
        [DefaultValue(0.0)]
        public double TotalAmount { get; set; }
        
        [JsonPropertyName("created_at")]
        [DefaultValue(null)]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        [DefaultValue(null)]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
