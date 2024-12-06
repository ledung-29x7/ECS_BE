using AutoMapper;
using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository,IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRole();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            var role = await _roleRepository.GetRoleById(id);
            if (role == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto)
        {
            if (roleDto == null)
            {
                return BadRequest("Role data is invalid.");
            }
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.AddRole(role);
            return Ok("Create Role Success");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest("Role ID mismatch.");
            }

            var existingRole = await _roleRepository.GetRoleById(id);
            if (existingRole == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }

            await _roleRepository.UpdateRole(role);
            return Ok("Update Role Success");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleRepository.GetRoleById(id);
            if (role == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }

            await _roleRepository.DeleteRole(id);
            return Ok("Delete Role Success");
        }


    }
}
