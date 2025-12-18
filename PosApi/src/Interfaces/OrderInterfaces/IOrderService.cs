namespace PosApi.Interfaces
{
    using PosApi.DTOs;
    using PosApi.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken = default);
        Task<PaginationResponse<OrderResponseDto>> GetAllOrdersAsync(int page, int limit, CancellationToken cancellationToken = default);
    }
}
