using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ECS.DAL.Repositorys
{
    public class OrderDetailReponsitory : IOrderDetailReponsitory
    {
        private readonly ECSDbContext ecSDbContext;

        public OrderDetailReponsitory(ECSDbContext ecSDbContext)
        {
            this.ecSDbContext = ecSDbContext;
        }
        public async Task<List<OrderDetail>> GetAllOrderDetails()
        {
            return await ecSDbContext.orderDetails
                  .FromSqlRaw("EXEC dbo.GetAllOrderDetails")
                  .ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailById(int orderDetailId)
        {
            var param = new SqlParameter("@OrderDetailId", orderDetailId);
            var result = await ecSDbContext.orderDetails
                .FromSqlRaw("EXEC dbo.GetOrderDetailById @OrderDetailId", param)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            var OrderId_Param = new SqlParameter("@OrderId", orderDetail.OrderId);
            var ProductId_Param = new SqlParameter("@ProductId", orderDetail.ProductId);
            var Quantity_Param = new SqlParameter("@Quantity", orderDetail.Quantity);
            var TotalPrice_Param = new SqlParameter("@TotalPrice", orderDetail.TotalPrice);
           

            await ecSDbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.AddOrderDetail @OrderId ,@ProductId ,@Quantity ,@TotalPrice", OrderId_Param , ProductId_Param , Quantity_Param , TotalPrice_Param
                );

        }

        public async Task UpdateOrderDetail(OrderDetail orderDetail)
        {
            var OrderDetailId = new SqlParameter("@OrderDetailId", orderDetail.OrderDetailId);
            var OrderId_Param = new SqlParameter("@OrderId", orderDetail.OrderId);
            var ProductId_Param = new SqlParameter("@ProductId", orderDetail.ProductId);
            var Quantity_Param = new SqlParameter("@Quantity", orderDetail.Quantity);
            var TotalPrice_Param = new SqlParameter("@TotalPrice", orderDetail.TotalPrice);


            await ecSDbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.UpdateOrderDetail @OrderDetailId,@OrderId ,@ProductId ,@Quantity ,@TotalPrice", OrderId_Param, ProductId_Param, Quantity_Param,TotalPrice_Param ,OrderDetailId
                );
        }

        public async Task DeleteOrderDetail(int orderDetailId)
        {
            var param = new SqlParameter("@orderDetailId", orderDetailId);
            await ecSDbContext.Database.ExecuteSqlRawAsync("EXEC dbo.DeleteOrderDetail @orderDetailId", param);
        }
    }
}

