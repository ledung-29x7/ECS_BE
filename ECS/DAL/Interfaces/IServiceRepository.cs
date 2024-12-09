using ECS.Areas.Admin.Models;

namespace ECS.DAL.Interfaces
{
    public interface IServiceRepository
    {
        Task CreateService(Service service);
        Task<List<Service>> GetAllService();
        Task<Service> GetServiceById(int serviceId);
        Task UpdateService(Service service);
        Task DeleteService(int serviceId);

    }
}
