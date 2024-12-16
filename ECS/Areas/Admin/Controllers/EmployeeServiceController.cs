    using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECS.Areas.Admin.Models;
using ECS.Areas.Client.Models;
using ECS.DAL.Repositorys;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeServiceController : ControllerBase
    {
        private readonly IEmployeeServiceRepository _repository;

        public EmployeeServiceController(IEmployeeServiceRepository repository)
        {
            _repository = repository;
        }

        // GET: api/EmployeeService
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _repository.GetAllEmployeeAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/EmployeeService/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _repository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new { message = "Employee Service not found" });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/EmployeeService
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ECS.Areas.Admin.Models.EmployeeService employeeService)
        {
            try
            {
                if (employeeService == null)
                    return BadRequest(new { message = "Invalid Employee Service data" });

                await _repository.AddEmployeeAsync(employeeService);
                return CreatedAtAction(nameof(GetById), new { id = employeeService.EmployeeServiceId }, employeeService);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/EmployeeService/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ECS.Areas.Admin.Models.EmployeeService employeeService)
        {
            if (id != employeeService.EmployeeServiceId)
            {
                return BadRequest("Product ID mismatch.");
            }

            if (employeeService == null)
            {
                return BadRequest("Product object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.UpdateEmployeeAsync(employeeService);
                return Ok("Product updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/EmployeeService/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingEmployeeService = await _repository.GetEmployeeByIdAsync(id);
                if (existingEmployeeService == null)
                    return NotFound(new { message = "Employee Service not found" });

                await _repository.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

