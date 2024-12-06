using ECS.Areas.Authen.Models;

namespace ECS.DAL.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleById(int roleId);
        Task AddRole(Role role);
        Task DeleteRole(int roleId);
        Task UpdateRole(Role role);

        Task<List<Role>> GetAllRole();
    }
}
