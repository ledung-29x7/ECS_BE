using ECS.Areas.EmployeeService.Models;
using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IOrderReponsitory
    {
        Task AddOrderWithDetails(OrderDto order, List<OrderDetailDto> orderDetails);
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);

        Task<List<GetOrderDetalByOrderId>> GetOrderdetailByOrderId(int orderId);
    }
}
