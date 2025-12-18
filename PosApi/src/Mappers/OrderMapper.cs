namespace PosApi.Mappers
{
    using PosApi.Entities;
    using PosApi.DTOs;

    public static class OrderMapper
    {
        /// <summary>
        /// Map Order entity to OrderResponseDto
        /// </summary>
        public static OrderResponseDto ToOrderResponseDto(this Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.Items.Select(item => item.ToOrderItemDto()).ToList()
            };
        }

        /// <summary>
        /// Map OrderItem entity to OrderItemDto
        /// </summary>
        public static OrderItemDto ToOrderItemDto(this OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            return new OrderItemDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                ProductPrice = orderItem.ProductPrice,
                Quantity = orderItem.Quantity,
                TotalPrice = orderItem.TotalPrice,
            };
        }
        /// <summary>
        /// Map collection of Orders to OrderResponseDto list
        /// </summary>
        public static List<OrderResponseDto> ToOrderResponseDtoList(this IEnumerable<Order> orders)
        {
            if (orders == null)
                throw new ArgumentNullException(nameof(orders));

            return orders.Select(order => order.ToOrderResponseDto()).ToList();
        }
    }
}
