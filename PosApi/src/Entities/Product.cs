namespace PosApi.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; } = 0.0;
        [Required]
        [DefaultValue(0)]
        public int Quantity { get; set; } = 0;
        [MaxLength(100)]
        [Required]
        public string Category { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
