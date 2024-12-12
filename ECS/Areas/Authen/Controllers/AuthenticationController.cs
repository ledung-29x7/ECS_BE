using AutoMapper;
using Azure.Core;
using ECS.Areas.Authen.Models;
using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.IdentityModel.Tokens.Jwt;

namespace ECS.Areas.Authen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenBlacklistRepository _tokenBlacklistRepository;
        private readonly IMapper _mapper;

        public AuthenticationController(IEmployeeRepository employeeRepository,
                                        IMapper mapper,
                                        IAuthenticationRepository authenticationRepository,
                                        IRoleRepository roleRepository,
                                        ITokenBlacklistRepository tokenBlacklistRepository) 
        {
            _employeeRepository = employeeRepository;
            _authenticationRepository = authenticationRepository;
            _roleRepository = roleRepository;
            _tokenBlacklistRepository = tokenBlacklistRepository;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterEmployeeDto registerDto)
        {
            var images = new List<ImageTable>();

            foreach (var imageFile in registerDto.ImageFiles)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    string imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    images.Add(new ImageTable { ImageBase64 = imageBase64 });
                }
            }

            var existingUser = await _employeeRepository.GetEmployeeByEmail(registerDto.Email);
            if (existingUser != null)
                return BadRequest("Email is already registered.");

            var employee = _mapper.Map<Employee>(registerDto);
            employee.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            await _employeeRepository.AddEmployeeWithImagesAsync(employee, images);

            return Ok("Employee registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginEmployeeDto request)
        {
            var employee = await _employeeRepository.GetEmployeeByEmail(request.Email);

            if (employee == null || !BCrypt.Net.BCrypt.Verify(request.Password, employee.Password))
                return Unauthorized("Invalid credentials.");

            var role = await _roleRepository.GetRoleById(employee.RoleId);
            var roleName = role.RoleName;
            var token = _authenticationRepository.GenerateJwtToken(employee, roleName);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                Path = "/",
                HttpOnly = false,
                SameSite = SameSiteMode.None,
                Secure = true
            };

            Response.Cookies.Append("token", token, cookieOptions);

            return Ok(new
            {
                UserName = employee.FirstName,
                Role = role.RoleName,
                EmployeeID = employee.EmployeeId,
                token
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token not provided");

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var expiration = jwtToken.ValidTo;

            // Thêm token vào Redis blacklist với thời gian hết hạn
            await _tokenBlacklistRepository.AddTokenToBlacklistAsync(token, expiration);

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return Ok(employees);
        }
    }
}
