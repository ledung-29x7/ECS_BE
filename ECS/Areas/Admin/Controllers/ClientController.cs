using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        private readonly IEmailService _emailService;

        public ClientController(IClientRepository clientRepository,
                                        IMapper mapper,
                                        IAuthenticationRepository authenticationRepository,
                                        ITokenBlacklistRepository tokenBlacklistRepository,
                                        IEmailService emailService)
        {
            _clientRepository = clientRepository;
            _authenticationRepository = authenticationRepository;
            _tokenBlacklistRepository = tokenBlacklistRepository;
            _mapper = mapper;
            _emailService = emailService;
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
            var subject = "Your Enrollment Code";
            var body = $"<!DOCTYPE html>\r\n<html lang=\"vi\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Thông Tin Tài Khoản Khách Hàng</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f4f4f4;\r\n        }}\r\n        .container {{\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            padding: 20px;\r\n            background-color: #fff;\r\n            border-radius: 10px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }}\r\n        h1 {{\r\n            color: #007BFF;\r\n            font-size: 24px;\r\n        }}\r\n        p {{\r\n            margin: 10px 0;\r\n        }}\r\n        .details {{\r\n            background-color: #f0f8ff;\r\n            padding: 15px;\r\n            border-left: 5px solid #007BFF;\r\n            margin: 20px 0;\r\n            border-radius: 5px;\r\n        }}\r\n        a {{\r\n            color: #007BFF;\r\n            text-decoration: none;\r\n        }}\r\n        a:hover {{\r\n            text-decoration: underline;\r\n        }}\r\n        .footer {{\r\n            margin-top: 20px;\r\n            font-size: 14px;\r\n            color: #555;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Thông Tin Tài Khoản Đăng Nhập Của Quý Khách</h1>\r\n        \r\n        <p>Kính gửi <strong>{registerDto.ContactPerson}</strong>,</p>\r\n\r\n        <p>Chúng tôi xin thông báo tài khoản của quý khách đã được tạo thành công trên hệ thống của <strong>ECS</strong>. Dưới đây là thông tin đăng nhập của quý khách:</p>\r\n\r\n        <div class=\"details\">\r\n            <p><strong>Tên đăng nhập:</strong>{registerDto.Email}</p>\r\n            <p><strong>Mật khẩu:</strong>{registerDto.Password}</p>\r\n        </div>\r\n\r\n        <p>Quý khách vui lòng truy cập vào hệ thống thông qua đường link sau để đăng nhập và thay đổi mật khẩu ngay khi có thể: <br>\r\n        <a href=\"[Link đăng nhập]\">[Link đăng nhập]</a></p>\r\n\r\n        <p>Nếu có bất kỳ thắc mắc hoặc cần hỗ trợ thêm, quý khách vui lòng liên hệ với chúng tôi qua <a href=\"mailto:ecs@gmail.com\">ecs@gmail.com</a> hoặc <strong>0973 111 086</strong>.</p>\r\n\r\n        <div class=\"footer\">\r\n            <p>Trân trọng,</p>\r\n            <p><strong>Le Chung Dung</strong></p>\r\n            <p><strong>ECS</strong></p>\r\n            <p> 8A Tôn Thất Thuyết, P.Mỹ Đình 2, Q.Nam Từ Liêm, Hà Nội</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>";
            await _emailService.SendEmailAsync(registerDto.Email,subject,body);
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
                UserId = client.ClientId,
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

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            try
            {
                await _clientRepository.ChangePassword(clientId, changePasswordDto.OldPassword, newPasswordHash);
                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Models.Client>>> GetAllClient()
        //{
        //    var clients = await _clientRepository.GetAllClient();
        //    return Ok(clients);
        //}

        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] int pageNumber = 1, [FromQuery] string searchTerm = null)
        {
            var (clients, totalRecords, totalPages) = await _clientRepository.GetAllClientAndSearchAsync(pageNumber, searchTerm);

            return Ok(new
            {
                Clients = clients,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetClientById(Guid id)
        {
            var client = await _clientRepository.GetClientById(id);
            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }
            return Ok(client);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] Models.Client client)
        {
            if (id != client.ClientId)
            {
                return BadRequest("Role ID mismatch.");
            }

            var existingRole = await _clientRepository.GetClientById(id);
            if (existingRole == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }

            await _clientRepository.UpdateClient(client);
            return Ok("Update Client Success");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _clientRepository.GetClientById(id);
            if (role == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }

            await _clientRepository.DeleteClient(id);
            return Ok("Delete Client Success");
        }


    }
}
