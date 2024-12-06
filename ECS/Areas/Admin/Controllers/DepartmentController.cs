using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository,
                                    IMapper mapper) 
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departments>>> GetAllDepartments()
        {
            var departments = await _departmentRepository.GetAllDepartments();
            if (departments == null || departments.Count == 0)
            {
                return NotFound("No departments found.");
            }

            return Ok(departments);
        }

        [HttpPost]
        public async Task<ActionResult<Departments>> AddDepartment([FromBody] DeparmentCreateDto departmentDto)
        {
            if (departmentDto == null)
            {
                return BadRequest("Department data is required.");
            }

            // Ánh xạ từ DTO sang entity
            var department = _mapper.Map<Departments>(departmentDto);

            // Truyền department vào repository
            await _departmentRepository.AddDepartments(department);

            return Ok("Create Department Success");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Departments department)
        {
            if (department == null || id != department.DepartmentID)
            {
                return BadRequest("Department data is invalid.");
            }

            await _departmentRepository.UpdateDepartments(department);

            return NoContent();  
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentRepository.GetDepartmentsById(id);

            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            await _departmentRepository.DeleteDepartments(id);

            return NoContent(); 
        }
    }
}
