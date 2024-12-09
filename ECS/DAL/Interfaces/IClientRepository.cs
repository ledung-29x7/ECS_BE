using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;

namespace ECS.DAL.Interfaces
{
    public interface IClientRepository
    {
        Task RegisterClient(Client client);
        Task<Client> GetClientByEmail(string email);
        Task<Client> GetClientById(Guid clientId);
        Task<List<Client>> GetAllClient();
        Task DeleteClient(Guid clientId);
        Task UpdateClient(Client client);
    }
}
