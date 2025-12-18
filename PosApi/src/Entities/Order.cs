namespace PosApi.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using PosApi.Entities;
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string OrderNumber { get; set; } = string.Empty;
        [Required]
        public double TotalAmount { get; set; } = 0.0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
