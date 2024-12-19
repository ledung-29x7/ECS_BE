using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace ECS.Areas.Authen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{employeeId}/worklist")]
        public async Task<IActionResult> GetEmployeeWorkList(Guid employeeId)
        {
            var workList = await _employeeRepository.GetEmployeeWorkListByIdAsync(employeeId);
            if (workList == null || !workList.Any())
            {
                return NotFound("No worklist found for this employee.");
            }
            return Ok(workList);
        }

        [HttpPost("available")]
        public async Task<IActionResult> GetAvailableEmployees([FromBody] EmployeeAvailableRequestDto request)
        {
            try
            {
                var employees = await _employeeRepository.GetEmployeeAvailables(request.ProductId, request.RequiredEmployees);

                if (employees == null || employees.Count == 0)
                {
                    return NotFound(new { Message = "No available employees found." });
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching employees.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStatus>> GetById(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            await _employeeRepository.UpdateEmployee(employee);
            return NoContent();
        }
    }

}
