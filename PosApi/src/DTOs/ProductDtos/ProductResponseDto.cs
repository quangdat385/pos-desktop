namespace PosApi.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Text.Json.Serialization;
    public class ProductResponseDto
    {
        [JsonPropertyName("id")]
        [DefaultValue(1)]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        [DefaultValue("Sample Product")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("price")]
        [DefaultValue(0.0)]
        public double Price { get; set; }
        [JsonPropertyName("quantity")]
        [DefaultValue(0)]
        public int Quantity { get; set; }
        [JsonPropertyName("category")]
        [DefaultValue("General")]
        public string Category { get; set; } = string.Empty;
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
