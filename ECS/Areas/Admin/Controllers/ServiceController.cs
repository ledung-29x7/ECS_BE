using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository) 
        {
            _serviceRepository = serviceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var services = await _serviceRepository.GetAllService();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            try
            {
                var service = await _serviceRepository.GetServiceById(id);
                if (service == null)
                {
                    return NotFound($"Service with ID {id} not found.");
                }
                return Ok(service);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest("Service data is null.");
            }

            try
            {
                await _serviceRepository.CreateService(service);
                return Ok("Service created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] Service service)
        {
            if (service == null || service.ServiceId != id)
            {
                return BadRequest("Invalid service data.");
            }

            try
            {
                var existingService = await _serviceRepository.GetServiceById(id);
                if (existingService == null)
                {
                    return NotFound($"Service with ID {id} not found.");
                }

                await _serviceRepository.UpdateService(service);
                return Ok("Service updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var existingService = await _serviceRepository.GetServiceById(id);
                if (existingService == null)
                {
                    return NotFound($"Service with ID {id} not found.");
                }

                await _serviceRepository.DeleteService(id);
                return Ok("Service deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
