using BlazorAndRedis.Repositories;
using Microsoft.AspNetCore.Mvc;
using ProductService.Data.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var product = await _productRepository.GetProducts();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(int id)
        {
            var product = await _productRepository.GetProduct(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts([FromBody] Product product)
        {
            bool res = await _productRepository.CreateProducts(product);
            if (res)
            {
                return Ok("Successfully Created");
            }
            return StatusCode(StatusCodes.Status406NotAcceptable, "Not Created");
        }
    }
}
