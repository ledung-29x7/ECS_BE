using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductReponsitory productReponsitory;

        public ProductController(IProductReponsitory productReponsitory)
        {
            this.productReponsitory = productReponsitory;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
         
                var products = await productReponsitory.GetAllProduct();
            
                    return Ok(products);
             

        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await productReponsitory.AddProduct(product);
                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
