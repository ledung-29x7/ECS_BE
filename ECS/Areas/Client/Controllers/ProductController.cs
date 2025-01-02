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
using System.Text;
using System;
using System.Security.Cryptography;

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

        //[HttpPut("active/{id}")]
        //public async Task<IActionResult> ActiveProduct(Guid id)
        //{
        //    await _productReponsitory.ActiveProduct(id);
        //    var employee = await _productReponsitory.GetClientByProductId(id);
        //    var product = await _productReponsitory.GetProductById(id);
        //    var productId = product.ProductId;
        //    using (var sha256 = SHA256.Create())
        //    {
        //        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(productId.ToString()));
        //        string shortHash = BitConverter.ToString(hash).Replace("-", "").Substring(0, 8);
        //    }
        //    var EmployeeEmail = employee.Email;
        //    var subject = "Your Enrollment Code";
        //    var body = $"<!DOCTYPE html>\r\n<html lang=\"vi\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Xác Nhận Kích Hoạt Sản Phẩm</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f4f4f4;\r\n        }}\r\n        .container {{\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            padding: 20px;\r\n            background-color: #fff;\r\n            border-radius: 10px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }}\r\n        h1 {{\r\n            color: #007BFF;\r\n            font-size: 24px;\r\n        }}\r\n        p {{\r\n            margin: 10px 0;\r\n        }}\r\n        .details {{\r\n            background-color: #f0f8ff;\r\n            padding: 15px;\r\n            border-left: 5px solid #007BFF;\r\n            margin: 20px 0;\r\n            border-radius: 5px;\r\n        }}\r\n        a {{\r\n            color: #007BFF;\r\n            text-decoration: none;\r\n        }}\r\n        a:hover {{\r\n            text-decoration: underline;\r\n        }}\r\n        .footer {{\r\n            margin-top: 20px;\r\n            font-size: 14px;\r\n            color: #555;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Xác Nhận Sản Phẩm Đã Được Kích Hoạt</h1>\r\n        \r\n        <p>Kính gửi <strong>{employee.ContactPerson}</strong>,</p>\r\n\r\n        <p>Cảm ơn quý khách đã tin tưởng và gửi sản phẩm cho <strong>ECS</strong> để kinh doanh.</p>\r\n\r\n        <p>Chúng tôi vui mừng thông báo sản phẩm của quý khách đã được kích hoạt thành công trên hệ thống và sẵn sàng để bán.</p>\r\n\r\n        <div class=\"details\">\r\n            <p><strong>Tên sản phẩm:</strong> {product.ProductName}</p>\r\n            <p><strong>Ngày kích hoạt:</strong> {DateTime.Now}</p>\r\n            <p><strong>Mã sản phẩm:</strong>{shortHash}</p>\r\n        </div>\r\n\r\n        <p>Quý khách có thể kiểm tra trạng thái sản phẩm và theo dõi doanh thu thông qua cổng thông tin khách hàng tại: \r\n        <a href=\"[Link]\">[Link đến cổng thông tin]</a>.</p>\r\n\r\n        <p>Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua <a href=\"mailto:ecs@gmail.com\">ecs@gmail.com</a> hoặc <strong>0973 111 086</strong>.</p>\r\n\r\n        <div class=\"footer\">\r\n            <p>Trân trọng,</p>\r\n            <p><strong>Le Chung Dung</strong></p>\r\n            <p><strong>ECS</strong></p>\r\n            <p>8A Tôn Thất Thuyết, P.Mỹ Đình 2, Q.Nam Từ Liêm, Hà Nội</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>";
        //    await _emailService.SendEmailAsync(EmployeeEmail, subject, body);
        //    return Ok("Update Success");
        //}

        [HttpPut("active/{id}")]
        public async Task<IActionResult> ActiveProduct(Guid id)
        {
            // Kích hoạt sản phẩm
            await _productReponsitory.ActiveProduct(id);

            // Lấy thông tin sản phẩm và khách hàng
            var client = await _productReponsitory.GetClientByProductId(id);
            var product = await _productReponsitory.GetProductById(id);

            if (client == null || product == null)
            {
                return NotFound("Client or Product not found");
            }

            // Tạo mã sản phẩm ngắn gọn
            string shortHash = GenerateShortHash(product.ProductId.ToString());

            // Tạo nội dung email
            string emailBody = GenerateEmailBody(client.ContactPerson, product.ProductName, shortHash);

            // Gửi email
            await _emailService.SendEmailAsync(client.Email, "Your Enrollment Code", emailBody);

            return Ok("Update Success");
        }

        // Phương thức tạo mã sản phẩm ngắn gọn
        private string GenerateShortHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 8);
            }
        }

        // Phương thức tạo nội dung email
        private string GenerateEmailBody(string contactPerson, string productName, string shortHash)
        {
            return $@"
    <!DOCTYPE html>
    <html lang='vi'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Xác Nhận Kích Hoạt Sản Phẩm</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                line-height: 1.6;
                color: #333;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
            }}
            .container {{
                max-width: 600px;
                margin: 20px auto;
                padding: 20px;
                background-color: #fff;
                border-radius: 10px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            }}
            h1 {{
                color: #007BFF;
                font-size: 24px;
            }}
            .details {{
                background-color: #f0f8ff;
                padding: 15px;
                border-left: 5px solid #007BFF;
                margin: 20px 0;
                border-radius: 5px;
            }}
            .footer {{
                margin-top: 20px;
                font-size: 14px;
                color: #555;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h1>Xác Nhận Sản Phẩm Đã Được Kích Hoạt</h1>
            <p>Kính gửi <strong>{contactPerson}</strong>,</p>
            <p>Cảm ơn quý khách đã tin tưởng và gửi sản phẩm cho <strong>ECS</strong> để kinh doanh.</p>
            <p>Chúng tôi vui mừng thông báo sản phẩm của quý khách đã được kích hoạt thành công trên hệ thống và sẵn sàng để bán.</p>
            <div class='details'>
                <p><strong>Tên sản phẩm:</strong> {productName}</p>
                <p><strong>Ngày kích hoạt:</strong> {DateTime.Now}</p>
                <p><strong>Mã sản phẩm:</strong> {shortHash}</p>
            </div>
            <p>Quý khách có thể kiểm tra trạng thái sản phẩm và theo dõi doanh thu thông qua cổng thông tin khách hàng tại: 
            <a href='[Link]'>[Link đến cổng thông tin]</a>.</p>
            <p>Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua <a href='mailto:ecs@gmail.com'>ecs@gmail.com</a> hoặc <strong>0973 111 086</strong>.</p>
            <div class='footer'>
                <p>Trân trọng,</p>
                <p><strong>Le Chung Dung</strong></p>
                <p><strong>ECS</strong></p>
                <p>8A Tôn Thất Thuyết, P.Mỹ Đình 2, Q.Nam Từ Liêm, Hà Nội</p>
            </div>
        </div>
    </body>
    </html>";
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
        [HttpPost]
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
        [HttpGet("clients/{clientId}")]
        public async Task<IActionResult> GetProductsByClientIdWithSales(
    Guid clientId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] string searchTerm = null,
    [FromQuery] bool? isActive = null,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null)
        {
            // Gọi repository để lấy dữ liệu
            var (products, totalRecords, totalPages) = await _productReponsitory.GetProductsByClientIdWithSalesAsync(
                clientId,
                pageNumber,
                searchTerm,
                isActive,
                startDate,
                endDate);

            // Kiểm tra nếu không có sản phẩm nào được tìm thấy
            if (products == null || !products.Any())
            {
                return NotFound(new { Message = "No products found for the given criteria." });
            }

            // Tạo phản hồi dưới dạng JSON
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
