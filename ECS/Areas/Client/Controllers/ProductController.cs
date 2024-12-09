using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductbyId(Guid id)
        {

            var products = await productReponsitory.GetProductbyID(id);
                return Ok(products);
  
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await productReponsitory.DeleteProduct(id);
            return Ok();
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("Product ID mismatch.");
            }

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
                await productReponsitory.UpdateProduct(product);
                return Ok("Product updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
