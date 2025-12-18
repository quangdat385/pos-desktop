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
    [Route("api/v1/product")]
    [EnableRateLimiting("global")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        [ProducesResponseType(typeof(SuccessResponse<PaginationResponse<ProductResponseDto>, ProductController>), 200)]
        [ProducesResponseType(typeof(ErrorResponse<string>), 400)]
        [ProducesResponseType(typeof(ErrorResponse<string>), 404)]
        [HttpGet("get-list-product")]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 20,
            [FromQuery] string category = "All"
        )
        {
            var products = await _productService.GetAllProductsAsync(page, limit, category);
            var response = new SuccessResponse<PaginationResponse<ProductResponseDto>, ProductController>(
               products,
               "Products retrieved successfully",
               (int)AppSuccessCode.GET_SUCCESS,
               _logger);
            return Ok(response);
        }
    }
}
