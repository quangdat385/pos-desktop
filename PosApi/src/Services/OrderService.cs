namespace PosApi.Services
{
    using System;
    using System.Collections.Generic;
    using PosApi.Entities;
    using PosApi.Interfaces;
    using PosApi.DTOs;
    using PosApi.Mappers;
    using PosApi.Utils;
    using PosApi.Shared;
    using PosApi.Exceptions;
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<OrderResponseDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken = default)
        {
            string order_number ="ORD-" + RandomIntNumber.Generate(100000, 999999);
            List<OrderItem> items = orderDto.Items.Select(item => 
                {
                    return new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductPrice = item.ProductPrice,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                    };
                }).ToList();
            double totalAmountCalculated = 0;
            foreach (var item in items)
            {
                if (item.ProductId <= 0)
                {
                    throw new AppValidationException(
                        $"Product ID {item.ProductId} is invalid.", AppErrorCode.VALIDATION_ERROR, 400);
                }
                if (item.ProductPrice <= 0)
                {
                    throw new AppValidationException(
                        $"Product price for product ID {item.ProductId} must be greater than zero.", AppErrorCode.VALIDATION_ERROR, 400);
                }
                if (item.Quantity <= 0)
                {
                    throw new AppValidationException(
                        $"Quantity for product ID {item.ProductId} must be greater than zero.", AppErrorCode.VALIDATION_ERROR, 400);
                }
                if (item.TotalPrice != item.ProductPrice * item.Quantity)
                {
                    throw new AppValidationException(
                        $"Total price for product ID {item.ProductId} is incorrect.", AppErrorCode.VALIDATION_ERROR, 400);
                }
                totalAmountCalculated += item.TotalPrice;
            }
            if (totalAmountCalculated != orderDto.TotalAmount)
            {
                throw new AppValidationException(
                    "Total amount is incorrect.", AppErrorCode.VALIDATION_ERROR, 400);
            }
            var order = new Order
            {
                OrderNumber = order_number,
                TotalAmount = orderDto.TotalAmount,
                Items = items,
            };
            var createdOrder = await _orderRepository.CreateOrderAsync(order, cancellationToken);
            var response = OrderMapper.ToOrderResponseDto(createdOrder);
            return response;
        }
        public async Task<PaginationResponse<OrderResponseDto>> GetAllOrdersAsync(int page, int limit, CancellationToken cancellationToken = default)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(page, limit, cancellationToken);
            if (orders == null)
              {
                throw new AppValidationException("Order not found!", AppErrorCode.NOT_FOUND, 404);
            }
            var items= OrderMapper.ToOrderResponseDtoList(orders.Items);
            var response = new PaginationResponse<OrderResponseDto>
            {
                Items = items,
                CurrentPage = orders.CurrentPage,
                PageSize = orders.PageSize,
                TotalItems = orders.TotalItems,
                TotalPages = orders.TotalPages
            };
            return response;
        }
    }
}
