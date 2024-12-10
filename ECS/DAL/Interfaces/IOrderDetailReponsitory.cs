using ECS.Areas.EmployeeService.Models;

namespace ECS.DAL.Interfaces
{
    public interface IOrderDetailReponsitory
    {
        Task<List<OrderDetail>> GetAllOrderDetails();
        Task<OrderDetail> GetOrderDetailById(int orderDetailId);
        Task AddOrderDetail(OrderDetail orderDetail);
        Task UpdateOrderDetail(OrderDetail orderDetail);
        Task DeleteOrderDetail(int orderDetailId);
    }
}
