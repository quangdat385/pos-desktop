namespace PosApi.Repositories
{
    using PosApi.Entities;
    using PosApi.Interfaces;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PosApi.Data;
    using PosApi.DTOs;
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<PaginationResponse<Product>> GetAllProductsAsync(int page, int limit, string category, CancellationToken cancellationToken = default)
        {
            var query = _db.Products.AsQueryable();
            var distinctCategories = await query.Select(p => p.Category).Distinct().ToListAsync(cancellationToken);
            // If category is provided and not "all", filter by category
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(p => p.Category == category);
            }
            var totalItems = await query.CountAsync(cancellationToken);

            var products = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);
            var totalPages = limit > 0 ? (int)Math.Ceiling((double)totalItems / limit) : 1;
            var categories = new List<string>();
            categories.Add("All");
            categories.AddRange(distinctCategories);
            return new PaginationResponse<Product>
            {
                Items = products,
                CurrentPage = page,
                PageSize = limit,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Categories = categories
            };
        }
    }
}
