using ECS.Areas.EmployeeService.Models;

namespace ECS.DAL.Interfaces
{
    public interface IOrderReponsitory
    {
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);
    }
}
