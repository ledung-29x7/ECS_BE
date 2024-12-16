using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }

}
