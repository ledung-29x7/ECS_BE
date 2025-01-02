using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusRepository _orderStatusRepository;

        public OrderStatusController(IOrderStatusRepository orderStatusRepository)
        {
            _orderStatusRepository = orderStatusRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderStatus>>> GetAllOrderStatus()
        {
            var orderStatuses = await _orderStatusRepository.GetAllOrderStatus();
            return Ok(orderStatuses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStatus>> GetOrderStatusById(int id)
        {
            var orderStatus = await _orderStatusRepository.GetOrderStatusById(id);
            if (orderStatus == null)
            {
                return NotFound($"orderStatus with ID {id} not found.");
            }
            return Ok(orderStatus);
        }
    }
}
