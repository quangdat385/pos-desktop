namespace PosApi.Repositories
{
    using PosApi.Entities;
    using PosApi.Data;
    using PosApi.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using PosApi.DTOs;
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var nowUtc = DateTime.Now;
                var createdOrder = new Order
                {
                    OrderNumber = order.OrderNumber,
                    TotalAmount = order.TotalAmount,
                    CreatedAt = nowUtc,
                    UpdatedAt = nowUtc,
                };
                _db.Orders.Add(createdOrder);
                await _db.SaveChangesAsync(cancellationToken);
                List<OrderItem> orderItems = order.Items.Select(item =>
                    {
                        return new OrderItem
                        {
                            OrderId = createdOrder.Id,
                            ProductId = item.ProductId,
                            ProductPrice = item.ProductPrice,
                            Quantity = item.Quantity,
                            TotalPrice = item.TotalPrice,
                            CreatedAt = nowUtc,
                            UpdatedAt = nowUtc,
                        };
                    }).ToList();
                var productIds = orderItems.Select(oi => oi.ProductId).ToList();
                var existingProducts = await _db.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync(cancellationToken);
                foreach (var item in orderItems)
                {
                    var product = existingProducts.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                    {
                        throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
                    }
                    if (product.Quantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product ID {item.ProductId}.");
                    }
                    product.Quantity -= item.Quantity;
                    item.ProductPrice = product.Price;
                    item.TotalPrice = item.ProductPrice * item.Quantity;
                    _db.Products.Update(product);
                }
                await _db.SaveChangesAsync(cancellationToken);

                _db.OrderItems.AddRange(orderItems);

                await _db.SaveChangesAsync(cancellationToken);

                await tx.CommitAsync(cancellationToken);
                createdOrder.Items = orderItems;
                return createdOrder;
            }
            catch
            {
                await tx.RollbackAsync(cancellationToken);
                throw;
            }
        }
        public async Task<PaginationResponse<Order>> GetAllOrdersAsync(int page, int limit, CancellationToken cancellationToken = default)
        {
            var query = _db.Orders.AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var orders = await query.
                Include(o => o.Items).
                Skip((page - 1) * limit).
                Take(limit).
                OrderByDescending(o => o.CreatedAt).
                ToListAsync(cancellationToken);
            var totalPages = limit > 0 ? (int)Math.Ceiling((double)totalItems / limit) : 1;

            return new PaginationResponse<Order>
            {
                Items = orders,
                CurrentPage = page,
                PageSize = limit,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}

