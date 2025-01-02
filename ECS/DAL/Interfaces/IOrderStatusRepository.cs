using ECS.Areas.EmployeeService.Models;

namespace ECS.DAL.Interfaces
{
    public interface IOrderStatusRepository
    {
        Task<List<OrderStatus>> GetAllOrderStatus();
        Task<OrderStatus> GetOrderStatusById(int id);
    }
}
