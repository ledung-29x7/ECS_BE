using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public AdminController(IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IDepartmentRepository departmentRepository) 
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpPost("update-role")]
        public async Task<IActionResult> UpdateEmployeeRole([FromBody] UpdateEmployeeRole request)
        {
            await _employeeRepository.UpdateEmployeeRole(request.EmployeeId, request.RoleId);
            return Ok("Role updated successfully");
        }

        [HttpPost("update-department")]
        public async Task<IActionResult> UpdateDepartmentForEmployee([FromBody] UpdateDepartmentEmployee request)
        {
            await _employeeRepository.UpdateDepartmentForEmployee(request.EmployeeId, request.DepartmentId);
            return Ok("Department updated successfully");
        }

        [HttpPost("set-manager")]
        public async Task<IActionResult> SetManagerForDepartment([FromBody] SetManagerForDepartment request)
        {
            await _departmentRepository.SetManagerForDepartment(request.DepartmentId, request.ManagerId);
            return Ok("Set manager for department success");
        }

        [HttpPost("delete-employee")]
        public async Task<IActionResult> DeleteEmployee(Guid EmployeeId) 
        {
            await _employeeRepository.DeleteEmployeeAndUnsetManager(EmployeeId);
            return Ok("DeleteEmployee Success");
        }



    }
}
