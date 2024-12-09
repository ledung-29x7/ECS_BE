using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ECSDbContext _context;

        public ServiceRepository(ECSDbContext context) 
        {
            _context = context; 
        }
        public async Task CreateService(Service service)
        {
            var ServiceName_param = new SqlParameter("@ServiceName", service.ServiceName);
            var CostPerDay_param = new SqlParameter("@CostPerDay", service.CostPerDay);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE CreateService @ServiceName, @CostPerDay", ServiceName_param, CostPerDay_param);
        }

        public async Task DeleteService(int serviceId)
        {
            var id_Param = new SqlParameter("@ServiceId", serviceId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteService @ServiceId", id_Param);
        }

        public async Task<List<Service>> GetAllService()
        {
            return await Task.FromResult(_context.services.FromSqlRaw("EXECUTE dbo.GetAllService").ToList());

        }

        public async Task<Service> GetServiceById(int serviceId)
        {
            var id_param = new SqlParameter("@ServiceId", serviceId);
            var services = await _context.services
               .FromSqlRaw("EXECUTE dbo.GetServiceById @ServiceId", id_param)
               .ToListAsync();
            return services.FirstOrDefault();
        }

        public async Task UpdateService(Service service)
        {
            var id_param = new SqlParameter("@ServiceId", service.ServiceId);
            var ServiceName_param = new SqlParameter("@ServiceName", service.ServiceName);
            var CostPerDay_param = new SqlParameter("@CostPerDay", service.CostPerDay);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE UpdateService @ServiceId, @ServiceName, @CostPerDay", id_param,ServiceName_param, CostPerDay_param);
        }
    }
}
