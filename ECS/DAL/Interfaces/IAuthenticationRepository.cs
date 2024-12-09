using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;

namespace ECS.DAL.Interfaces
{
    public interface IAuthenticationRepository
    {
        string GenerateJwtToken(Employee employee, string roleName);

        string GenerateJwtTokenForClient(Client client);
    }
}
