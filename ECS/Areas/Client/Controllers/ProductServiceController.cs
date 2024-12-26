using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductServiceController : ControllerBase
    {
        private readonly IProductServiceRepository _productServiceRepository;

        public ProductServiceController(IProductServiceRepository productServiceRepository)
        {
            _productServiceRepository = productServiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductServices()
        {
            try
            {
                var productServices = await _productServiceRepository.GetAllProductService();
                return Ok(productServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("Client/{clientId}")]
        public async Task<IActionResult> GetProductServiceByClientId(Guid clientId)
        {
            try
            {
                var productServices = await _productServiceRepository.GetProductServiceByClientId(clientId);
                if (productServices == null || productServices.Count == 0)
                {
                    return NotFound($"No ProductService found for ClientId: {clientId}");
                }
                return Ok(productServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Product/{producrId}")]
        public async Task<IActionResult> GetProductServiceByProductId(Guid producrId)
        {
            try
            {
                var productServices = await _productServiceRepository.GetProductServiceByProductId(producrId);
                if (productServices == null || productServices.Count == 0)
                {
                    return NotFound($"No ProductService found for ClientId: {producrId}");
                }
                return Ok(productServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductService([FromBody] ProductService productService)
        {
            if (productService == null)
            {
                return BadRequest("ProductService data is null.");
            }

            try
            {
                await _productServiceRepository.CreateProductService(productService);
                return Ok("ProductService created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductService(int id, [FromBody] ProductService productService)
        {
            if (productService == null || productService.ProductServiceId != id)
            {
                return BadRequest("Invalid ProductService data.");
            }

            try
            {
                await _productServiceRepository.UpdateProductService(productService);
                return Ok("ProductService updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
