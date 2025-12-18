namespace PosApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PosApi.Entities;
    using PosApi.DTOs;
    using PosApi.Interfaces;
    using PosApi.Shared;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.RateLimiting;

    [ApiController]
    [Route("api/v1/order")]
    [EnableRateLimiting("global")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        [ProducesResponseType(typeof(SuccessResponse<OrderResponseDto, OrderController>), 201)]
        [ProducesResponseType(typeof(ErrorResponse<string>), 400)]
        [ProducesResponseType(typeof(ErrorResponse<string>), 404)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(
            [FromBody] OrderDto orderDtoRequest
            )
        {
            var orderDto = new OrderDto();
            orderDto.TotalAmount = orderDtoRequest.TotalAmount;
            orderDto.Items = orderDtoRequest.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductPrice = item.ProductPrice,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
            }).ToList();
            var created = await _orderService.CreateOrderAsync(orderDto);
            var response = new SuccessResponse<OrderResponseDto, OrderController>(
                created,
                "Order created successfully",
                (int)AppSuccessCode.CREATE_SUCCESS,
                _logger);
            return Ok(response);
        }
        [ProducesResponseType(typeof(SuccessResponse<PaginationResponse<OrderResponseDto>, OrderController>), 200)]
        [ProducesResponseType(typeof(ErrorResponse<string>), 400)]
        [ProducesResponseType(typeof(ErrorResponse<string>), 404)]
        [HttpGet("get-list-order")]
        public async Task<IActionResult> GetAllOrders(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 20
        )
        {
            var orders = await _orderService.GetAllOrdersAsync(page, limit);
            var response = new SuccessResponse<PaginationResponse<OrderResponseDto>, OrderController>(
               orders,
               "Orders retrieved successfully",
               (int)AppSuccessCode.GET_SUCCESS,
               _logger);
            return Ok(response);
        }
    }

}
