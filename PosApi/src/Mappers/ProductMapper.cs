namespace PosApi.Mappers
{
    using PosApi.Entities;
    using PosApi.DTOs;
    public static class ProductMapper
    {
        /// <summary>
        /// Map Product entity to ProductResponseDto
        /// </summary>
        public static ProductResponseDto ToProductResponseDto(this Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        /// <summary>
        /// Map collection of Products to ProductResponseDto list
        /// </summary>
        public static List<ProductResponseDto> ToProductResponseDtoList(this IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            return products.Select(p => p.ToProductResponseDto()).ToList();
        }
    }
}
