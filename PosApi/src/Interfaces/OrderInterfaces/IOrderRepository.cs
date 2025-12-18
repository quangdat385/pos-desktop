namespace PosApi.Interfaces
{
    using PosApi.DTOs;
    using PosApi.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task<PaginationResponse<Order>> GetAllOrdersAsync(int page, int limit, CancellationToken cancellationToken = default);
    }
}
