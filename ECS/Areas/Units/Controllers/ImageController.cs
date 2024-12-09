using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Units.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        //[HttpPost("add-images")]
        //public async Task<IActionResult> AddImages([FromBody] List<ImageTable> images)
        //{
        //    await _imageRepository.AddImages(images);
        //    return Ok("Images added successfully");
        //}

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetImageByEmployeeId(Guid employeeId)
        {
            var images = await _imageRepository.GetImageByEmloyeeId(employeeId);
            return Ok(images);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetImageByProductId(Guid productId)
        {
            var images = await _imageRepository.GetImageByProductId(productId);
            return Ok(images);
        }

        [HttpDelete("employee/{employeeId}")]
        public async Task<IActionResult> DeleteImageByEmployeeId(Guid employeeId)
        {
            await _imageRepository.DeleteImageByEmployeeId(employeeId);
            return Ok("Images deleted successfully");
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteImageByProductId(Guid productId)
        {
            await _imageRepository.DeleteImageByProductId(productId);
            return Ok("Images deleted successfully");
        }

        [HttpPost("add-product-images/{productId}")]
        public async Task<IActionResult> AddImagesToProduct(Guid productId, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            var images = new List<ImageTable>();

            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                images.Add(new ImageTable
                {
                    ImageBase64 = base64Image
                });
            }

            await _imageRepository.AddImagesToProduct(productId, images);
            return Ok("Images added to product successfully");
        }

        [HttpPost("add-employee-images/{employeeId}")]
        public async Task<IActionResult> AddImagesToEmployee(Guid employeeId, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            var images = new List<ImageTable>();

            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                images.Add(new ImageTable
                {
                    ImageBase64 = base64Image
                });
            }

            await _imageRepository.AddImagesToEmployee(employeeId, images);
            return Ok("Images added to employee successfully");
        }
    }
}
