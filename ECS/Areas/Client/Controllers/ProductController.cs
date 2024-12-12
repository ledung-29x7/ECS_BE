using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NuGet.Protocol.Core.Types;

namespace ECS.Areas.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductReponsitory _productReponsitory;
        private readonly IMapper _mapper;

        public ProductController(IProductReponsitory productReponsitory, IMapper mapper)
        {
            _productReponsitory = productReponsitory;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productReponsitory.GetAllProduct();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productReponsitory.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            await _productReponsitory.UpdateProductAsync(product);
            return Ok("Update Success");
        }
        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            var images = new List<ImageTable>();

            foreach (var imageFile in request.ImageFiles)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    string imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    images.Add(new ImageTable { ImageBase64 = imageBase64 });
                }
            }
            var product = _mapper.Map<Product>(request);
            await _productReponsitory.AddProductWithImageAsync(product, images);

            return Ok(new { Message = "Product and images added successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            
            await _productReponsitory.DeleteProductAsync(id);
            return Ok("Delete Success");
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetProductsByClientId(Guid clientId)
        {
            var products = await _productReponsitory.GetProductsByClientIdAsync(clientId);
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }


    }
}
