namespace PosApi.Services
{
    using PosApi.Entities;
    using PosApi.Interfaces;
    using PosApi.DTOs;
    using PosApi.Mappers;
    using PosApi.Shared;
    using PosApi.Exceptions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<PaginationResponse<ProductResponseDto>> GetAllProductsAsync(int page, int limit, string category, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllProductsAsync(page, limit, category, cancellationToken);
            if (products == null)
            {
                throw new AppValidationException("Products not found!", AppErrorCode.NOT_FOUND, 404);
            }
            var items = ProductMapper.ToProductResponseDtoList(products.Items);
            var response = new PaginationResponse<ProductResponseDto>
            {
                Items = items,
                CurrentPage = products.CurrentPage,
                PageSize = products.PageSize,
                TotalItems = products.TotalItems,
                TotalPages = products.TotalPages,
                Categories = products.Categories
            };
            return response;
        }
    }
}
