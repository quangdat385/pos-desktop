namespace PosApi.DTOs
{
    using System;
    using System.Text.Json.Serialization;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class OrderItemDto
    {
        [JsonPropertyName("id")]
        [DefaultValue(null)]
        public int? Id
        {
            get; set;
        }= null;

        [JsonPropertyName("order_id")]
        [DefaultValue(null)]
        public int? OrderId
        {
            get; set;
        }= null;

        [JsonPropertyName("product_id")]
        [Required]
        public int ProductId
        {
            get; set;
        }

        [JsonPropertyName("product_price")]
        [Required]
        public double ProductPrice
        {
            get; set;
        }

        [JsonPropertyName("quantity")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least1.")]
        public int Quantity
        {
            get; set;
        }

        [JsonPropertyName("total_price")]
        [Required]
        public double TotalPrice
        {
            get; set;
        }
    }
}
