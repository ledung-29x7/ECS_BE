using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Client.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStatusController : ControllerBase
    {
        private readonly IProductStatusRepository _repository;

        public ProductStatusController(IProductStatusRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ProductStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStatus>>> GetAll()
        {
            var productStatuses = await _repository.GetAllProductStatus();
            return Ok(productStatuses);
        }

        // GET: api/ProductStatus/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStatus>> GetById(int id)
        {
            var productStatus = await _repository.GetProductStatusById(id);
            if (productStatus == null)
            {
                return NotFound();
            }
            return Ok(productStatus);
        }

        // POST: api/ProductStatus
        [HttpPost]
        public async Task<IActionResult> Add(ProductStatus productStatus)
        {
            await _repository.AddProductStatus(productStatus);
            return CreatedAtAction(nameof(GetById), new { id = productStatus.StatusId }, productStatus);
        }

        // PUT: api/ProductStatus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductStatus productStatus)
        {
            if (id != productStatus.StatusId)
            {
                return BadRequest();
            }

            await _repository.UpdateProductStatus(productStatus);
            return NoContent();
        }

        // DELETE: api/ProductStatus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteProductStatus(id);
            return NoContent();
        }
    }
}
