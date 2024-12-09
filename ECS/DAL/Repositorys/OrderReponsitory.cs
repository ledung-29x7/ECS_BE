using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class OrderReponsitory : IOrderReponsitory
    {
        private readonly ECSDbContext _context;

        public OrderReponsitory(ECSDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.order
                .FromSqlRaw("EXEC dbo.GetAllOrders")
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var param = new SqlParameter("@OrderId", id);
            var result = await _context.order
                .FromSqlRaw("EXEC dbo.GetOrderById @OrderId", param)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task AddOrder(Order order)
        {
            var callIdParam = new SqlParameter("@CallId", order.CallId);
            var ordererParam = new SqlParameter("@Orderer", order.Orderer);
            var totalAmountParam = new SqlParameter("@TotalAmount", order.TotalAmount);
            var recipientNameParam = new SqlParameter("@Recipient_Name", order.Recipient_Name);
            var recipientPhoneParam = new SqlParameter("@Recipient_Phone", order.Recipient_Phone);
            var recipientAddressParam = new SqlParameter("@Recipient_Address", order.Recipient_Address);
            var orderStatusParam = new SqlParameter("@OrderStatus", order.OrderStatus);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.AddOrder @CallId, @Orderer, @TotalAmount, @Recipient_Name, @Recipient_Phone, @Recipient_Address, @OrderStatus",
                callIdParam, ordererParam, totalAmountParam, recipientNameParam, recipientPhoneParam, recipientAddressParam, orderStatusParam);
        }

        public async Task UpdateOrder(Order order)
        {
            var orderIdParam = new SqlParameter("@OrderId", order.OrderId);
            var callIdParam = new SqlParameter("@CallId", order.CallId);
            var ordererParam = new SqlParameter("@Orderer", order.Orderer);
            var totalAmountParam = new SqlParameter("@TotalAmount", order.TotalAmount);
            var recipientNameParam = new SqlParameter("@Recipient_Name", order.Recipient_Name);
            var recipientPhoneParam = new SqlParameter("@Recipient_Phone", order.Recipient_Phone);
            var recipientAddressParam = new SqlParameter("@Recipient_Address", order.Recipient_Address);
            var orderStatusParam = new SqlParameter("@OrderStatus", order.OrderStatus);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.UpdateOrder @OrderId, @CallId, @Orderer, @TotalAmount, @Recipient_Name, @Recipient_Phone, @Recipient_Address, @OrderStatus",
                orderIdParam, callIdParam, ordererParam, totalAmountParam, recipientNameParam, recipientPhoneParam, recipientAddressParam, orderStatusParam);
        }

        public async Task DeleteOrder(int id)
        {
            var param = new SqlParameter("@OrderId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.DeleteOrder @OrderId", param);
        }
    }
}

