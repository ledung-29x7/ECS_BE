using ECS.Areas.EmployeeService.Models;

namespace ECS.Dtos
{
    public class AddOrderWithDetailsRequest
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
