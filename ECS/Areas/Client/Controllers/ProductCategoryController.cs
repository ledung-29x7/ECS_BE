using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly  IProductCategoryReponsitory productCategoryReponsitory;

        public ProductCategoryController(IProductCategoryReponsitory productCategoryReponsitory)
        {
            this.productCategoryReponsitory = productCategoryReponsitory;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetProductCategory()
        {

            var products = await productCategoryReponsitory.GetAllCategory();

            return Ok(products);


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategorybyId(int id)
        {

            var products = await productCategoryReponsitory.GetProductCategorybyID(id);
            return Ok(products);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            await productCategoryReponsitory.DeleteProductCategory(id);
            return Ok();
        }
        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategory productCategory)
        {
            if (productCategory == null)
            {
                return BadRequest("ProductCategory object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await productCategoryReponsitory.AddProductCategory(productCategory);
                return Ok("ProductCategory added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(int id, [FromBody] ProductCategory productCategory)
        {
            if (id != productCategory.CategoryId)
            {
                return BadRequest("Category ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await productCategoryReponsitory.UpdateProductCategory(productCategory);
                return Ok("Product category updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
