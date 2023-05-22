using Microsoft.AspNetCore.Mvc;
using MultiTenantsExample.Core.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet(Name = "GetProducts")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var productDetails = await _productService.GetByIdAsync(id);
        return Ok(productDetails);
    }

    [HttpPost(Name = "GetProducts")]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request)
    {
        return Ok(await _productService.CreateAsync(request.Name, request.Description, request.Rate));
    }

    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
    }
}
