using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECS.DAL.Repositorys
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(Employee employee, string roleName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, employee.FirstName),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateJwtTokenForClient(Client client)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, client.ClientName),
                new Claim(ClaimTypes.Email, client.Email),
                new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
