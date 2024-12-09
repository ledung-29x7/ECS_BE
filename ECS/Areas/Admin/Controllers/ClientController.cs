using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ITokenBlacklistRepository _tokenBlacklistRepository;
        private readonly IMapper _mapper;

        public ClientController(IClientRepository clientRepository,
                                        IMapper mapper,
                                        IAuthenticationRepository authenticationRepository,
                                        ITokenBlacklistRepository tokenBlacklistRepository) 
        {
            _clientRepository = clientRepository;
            _authenticationRepository = authenticationRepository;
            _tokenBlacklistRepository = tokenBlacklistRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterClientDto registerDto)
        {
            var existingUser = await _clientRepository.GetClientByEmail(registerDto.Email);
            if (existingUser != null)
                return BadRequest("Email is already registered.");

            var client = _mapper.Map<Models.Client>(registerDto);
            client.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            await _clientRepository.RegisterClient(client);

            return Ok("Client registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginClientDto request)
        {
            var client = await _clientRepository.GetClientByEmail(request.Email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(request.Password, client.Password))
                return Unauthorized("Invalid credentials.");


            var token = _authenticationRepository.GenerateJwtTokenForClient(client);
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
                UserName = client.ClientName,
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


    }
}
