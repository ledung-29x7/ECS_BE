using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace ECS.Areas.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductReponsitory _productReponsitory;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IProductSalesRepository _productSalesRepository;

        public ProductController(IProductReponsitory productReponsitory,IEmailService emailService, IMapper mapper, IProductSalesRepository productSalesRepository = null)
        {
            _productReponsitory = productReponsitory;
            _mapper = mapper;
            _productSalesRepository = productSalesRepository;
            _emailService = emailService;
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

        [HttpPut("active/{id}")]
        public async Task<IActionResult> ActiveProduct(Guid id)
        {
            await _productReponsitory.ActiveProduct(id);
            var employee = await _productReponsitory.GetClientByProductId(id);
            var EmployeeEmail = employee.Email;
            var subject = "Your Enrollment Code";
            var body = $"<!DOCTYPE html>\r\n<html lang=\"vi\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Xác Nhận Kích Hoạt Sản Phẩm</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f4f4f4;\r\n        }}\r\n        .container {{\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            padding: 20px;\r\n            background-color: #fff;\r\n            border-radius: 10px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }}\r\n        h1 {{\r\n            color: #007BFF;\r\n            font-size: 24px;\r\n        }}\r\n        p {{\r\n            margin: 10px 0;\r\n        }}\r\n        .details {{\r\n            background-color: #f0f8ff;\r\n            padding: 15px;\r\n            border-left: 5px solid #007BFF;\r\n            margin: 20px 0;\r\n            border-radius: 5px;\r\n        }}\r\n        a {{\r\n            color: #007BFF;\r\n            text-decoration: none;\r\n        }}\r\n        a:hover {{\r\n            text-decoration: underline;\r\n        }}\r\n        .footer {{\r\n            margin-top: 20px;\r\n            font-size: 14px;\r\n            color: #555;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Xác Nhận Sản Phẩm Đã Được Kích Hoạt</h1>\r\n        \r\n        <p>Kính gửi <strong>{employee.ContactPerson}</strong>,</p>\r\n\r\n        <p>Cảm ơn quý khách đã tin tưởng và gửi sản phẩm cho <strong>ECS</strong> để kinh doanh.</p>\r\n\r\n        <p>Chúng tôi vui mừng thông báo sản phẩm của quý khách đã được kích hoạt thành công trên hệ thống và sẵn sàng để bán.</p>\r\n\r\n        <div class=\"details\">\r\n            <p><strong>Tên sản phẩm:</strong> [Tên sản phẩm]</p>\r\n            <p><strong>Ngày kích hoạt:</strong> [Ngày kích hoạt]</p>\r\n            <p><strong>Mã sản phẩm:</strong> [Mã sản phẩm]</p>\r\n        </div>\r\n\r\n        <p>Quý khách có thể kiểm tra trạng thái sản phẩm và theo dõi doanh thu thông qua cổng thông tin khách hàng tại: \r\n        <a href=\"[Link]\">[Link đến cổng thông tin]</a>.</p>\r\n\r\n        <p>Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua <a href=\"mailto:[Email hỗ trợ]\">[Email hỗ trợ]</a> hoặc <strong>[Số điện thoại hỗ trợ]</strong>.</p>\r\n\r\n        <div class=\"footer\">\r\n            <p>Trân trọng,</p>\r\n            <p><strong>[Tên của bạn]</strong></p>\r\n            <p><strong>[Tên công ty của bạn]</strong></p>\r\n            <p>[Thông tin liên hệ công ty]</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>";
            await _emailService.SendEmailAsync(EmployeeEmail, subject, body);
            return Ok("Update Success");
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
        //public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        //{
        //    var images = new List<ImageTable>();

        //    foreach (var imageFile in request.ImageFiles)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await imageFile.CopyToAsync(memoryStream);
        //            string imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
        //            images.Add(new ImageTable { ImageBase64 = imageBase64 });
        //        }
        //    }
        //    var product = _mapper.Map<Product>(request);
        //    await _productReponsitory.AddProductWithImageAsync(product, images);

        //    return Ok(new { Message = "Product and images added successfully" });
        //}
        public async Task<IActionResult> AddProduct([FromForm] CreateProductRequest request, [FromForm] string productServicesJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _productReponsitory.AddProduct(request, productServicesJson);
                return Ok(new { Message = "Product added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {

            await _productReponsitory.DeleteProductAsync(id);
            return Ok("Delete Success");
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetProductsByClientId(
            Guid clientId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string searchTerm = null,
            [FromQuery] bool? isActive = null)
        {
            var (products, totalRecords, totalPages) = await _productReponsitory.GetProductsByClientIdAsync(clientId, pageNumber, searchTerm, isActive);

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            var response = new
            {
                Products = products,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("GetProductSalesAndStock")]
        public async Task<IActionResult> GetProductSalesAndStock([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var result = await _productSalesRepository.GetProductSalesAndStockAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetProducts(
    [FromQuery] int pageNumber = 1,
    [FromQuery] string searchTerm = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] bool? isActive = null)
        {
            // Gọi repository với các tham số mới
            var (products, totalRecords, totalPages) = await _productReponsitory.GetAllProductsAsync(pageNumber, searchTerm, minPrice, maxPrice, isActive);

            // Tạo response object
            var response = new
            {
                Products = products,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };

            // Trả về kết quả
            return Ok(response);
        }



    }
}
