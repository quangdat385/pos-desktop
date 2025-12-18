namespace PosApi.Interfaces
{
    using PosApi.DTOs;
    using System.Collections.Generic;
    public interface IProductService
    {
        Task<PaginationResponse<ProductResponseDto>> GetAllProductsAsync(int page, int limit, string category, CancellationToken cancellationToken = default);
    }
}
