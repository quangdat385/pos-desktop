namespace PosApi.Interfaces
{
    using PosApi.Entities;
    using System.Collections.Generic;
    using PosApi.DTOs;
    public interface IProductRepository
    {
        Task<PaginationResponse<Product>> GetAllProductsAsync(int page, int limit, string category, CancellationToken cancellationToken = default);
    }
}
