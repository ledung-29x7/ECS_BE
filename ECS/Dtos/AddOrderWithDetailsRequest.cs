using ECS.Areas.EmployeeService.Models;
using StackExchange.Redis;

namespace ECS.Dtos
{
    public class AddOrderWithDetailsRequest
    {
        public OrderDto Order { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }

    }
}
