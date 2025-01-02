using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ECSDbContext _context;

        public OrderStatusRepository(ECSDbContext context) 
        {
            _context = context;
        }
        public async Task<List<OrderStatus>> GetAllOrderStatus()
        {
            return await Task.FromResult(_context.orderStatuses.FromSqlRaw("EXECUTE dbo.GetAllOrderStatus").ToList());
        }

        public async Task<OrderStatus> GetOrderStatusById(int id)
        {
            var id_param = new SqlParameter("@StatusId", id);
            var orderStatuses = await _context.orderStatuses
               .FromSqlRaw("EXECUTE dbo.GetOrderStatusById @StatusId", id_param)
               .ToListAsync();
            return orderStatuses.FirstOrDefault();
        }
    }
}
