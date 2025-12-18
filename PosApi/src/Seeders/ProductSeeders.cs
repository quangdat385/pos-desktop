namespace PosApi.Seeders
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PosApi.Data;
    using PosApi.Entities;
    using Microsoft.EntityFrameworkCore;

    public static class ProductSeeders
    {
        public static async Task SeedProductsAsync(AppDbContext context)
        {
            // Ensure EF Core async extensions are available (AnyAsync)
            if (await context.Products.AnyAsync())
            {
                return; // Database has been seeded
            }
            var now = DateTime.UtcNow;
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Cà phê sữa", Price = 25000, Quantity = 5, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 2, Name = "Bánh mì thịt", Price = 30000, Quantity = 10, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 3, Name = "Trà đá", Price = 10000, Quantity = 20, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 4, Name = "Phở bò", Price = 40000, Quantity = 8, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 5, Name = "Nước suối", Price = 15000, Quantity = 15, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 6, Name = "Bún chả", Price = 35000, Quantity = 12, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 7, Name = "Sinh tố bơ", Price = 30000, Quantity = 7, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 8, Name = "Gỏi cuốn", Price = 20000, Quantity = 18, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 9, Name = "Cà phê đen", Price = 20000, Quantity = 9, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 10, Name = "Cháo gà", Price = 30000, Quantity = 14, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 11, Name = "Nước cam", Price = 25000, Quantity = 11, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 12, Name = "Bánh xèo", Price = 40000, Quantity = 6, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 13, Name = "Trà sữa", Price = 30000, Quantity = 13, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 14, Name = "Cơm tấm", Price = 35000, Quantity = 10, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 15, Name = "Nước dừa", Price = 20000, Quantity = 16, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 16, Name = "Mì Quảng", Price = 45000, Quantity = 5, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 17, Name = "Cà phê bạc xỉu", Price = 25000, Quantity = 8, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 18, Name = "Hủ tiếu", Price = 30000, Quantity = 12, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 19, Name = "Sữa chua uống", Price = 15000, Quantity = 20, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 20, Name = "Bánh bao", Price = 20000, Quantity = 15, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 21, Name = "Trà chanh", Price = 10000, Quantity = 18, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 22, Name = "Xôi gà", Price = 30000, Quantity = 9, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 23, Name = "Nước ép dưa hấu", Price = 25000, Quantity = 14, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 24, Name = "Bánh cuốn", Price = 35000, Quantity = 7, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 25, Name = "Cà phê trứng", Price = 30000, Quantity = 11, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 26, Name = "Lẩu cá kèo", Price = 50000, Quantity = 6, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 27, Name = "Soda chanh", Price = 20000, Quantity = 17, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 28, Name = "Bò kho", Price = 40000, Quantity = 10, Category = "Food", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 29, Name = "Trà hoa cúc", Price = 15000, Quantity = 19, Category = "Beverages", CreatedAt = now, UpdatedAt = now },
                new Product { Id = 30, Name = "Chè thập cẩm", Price = 25000, Quantity = 13, Category = "Food", CreatedAt = now, UpdatedAt = now }
            };
            
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
