﻿using System.Data;
using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
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

        public async Task<(List<Order> Orders, int TotalOrders, int TotalPages)> GetAllOrders(int pageNumber)
        {
            var totalOrdersParam = new SqlParameter
            {
                ParameterName = "@TotalOrders",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var totalPagesParam = new SqlParameter
            {
                ParameterName = "@TotalPages",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);

            var orders = await _context.order
                .FromSqlRaw("EXEC dbo.GetAllOrders @PageNumber, @TotalOrders OUTPUT, @TotalPages OUTPUT",
                    pageNumberParam, totalOrdersParam, totalPagesParam)
                .ToListAsync();

            int totalOrders = (int)totalOrdersParam.Value;
            int totalPages = (int)totalPagesParam.Value;

            return (orders, totalOrders, totalPages);
        }

        public async Task<Order> GetOrderById(int id)
        {
            var param = new SqlParameter("@OrderId", id);
            var result = await _context.order
                .FromSqlRaw("EXEC dbo.GetOrderById @OrderId", param)
                .ToListAsync();

            return result.FirstOrDefault();
        }
        public async Task AddOrderWithDetails(OrderDto order, List<OrderDetailDto> orderDetails)
        {
            order.TotalAmount = orderDetails.Sum(detail => detail.Quantity * detail.TotalPrice);

            var orderIdOutput = new SqlParameter("@OrderId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC dbo.AddOrder 
            @CallId, 
            @Orderer, 
            @TotalAmount, 
            @Recipient_Name, 
            @Recipient_Phone, 
            @Recipient_Address, 
            @OrderId OUTPUT",
                new SqlParameter("@CallId", order.CallId),
                new SqlParameter("@Orderer", order.Orderer),
                new SqlParameter("@TotalAmount", order.TotalAmount),
                new SqlParameter("@Recipient_Name", order.RecipientName),
                new SqlParameter("@Recipient_Phone", order.RecipientPhone),
                new SqlParameter("@Recipient_Address", order.RecipientAddress),
                orderIdOutput
            );

            int newOrderId = (int)orderIdOutput.Value;

            foreach (var detail in orderDetails)
            {
                var productPrice = await _context.product
                    .Where(p => p.ProductId == detail.ProductId)
                    .Select(p => p.Price)
                    .FirstOrDefaultAsync();

                if (productPrice == null || productPrice <= 0)
                {
                    throw new Exception($"Invalid price for product {detail.ProductId}");
                }


                detail.TotalPrice = detail.Quantity * (productPrice ?? 0);


                order.TotalAmount += detail.TotalPrice;

                var orderIdParam = new SqlParameter("@OrderId", newOrderId);
                var productIdParam = new SqlParameter("@ProductId", detail.ProductId);
                var quantityParam = new SqlParameter("@Quantity", detail.Quantity);
                var totalPriceParam = new SqlParameter("@TotalPrice", detail.TotalPrice);

                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC dbo.AddOrderDetail 
            @OrderId, 
            @ProductId, 
            @Quantity, 
            @TotalPrice",
                    orderIdParam, productIdParam, quantityParam, totalPriceParam
                );
            }

            await _context.Database.ExecuteSqlRawAsync(
                @"UPDATE [Order] SET TotalAmount = @TotalAmount WHERE OrderId = @OrderId",
                new SqlParameter("@TotalAmount", order.TotalAmount),
                new SqlParameter("@OrderId", newOrderId)
            );
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

        public async Task<List<GetOrderDetalByOrderId>> GetOrderdetailByOrderId(int orderId)
        {
            var orderId_Param = new SqlParameter("@OrderId", orderId);
            return await _context.getOrderDetalByOrderIds
                .FromSqlRaw("EXEC dbo.GetOrderdetailByOrderId @OrderId", orderId_Param)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrderByEmployeeId(Guid id)
        {
            return await _context.order
                 .FromSqlRaw("EXEC dbo.GetOrderByEmployeeId @EmployeeId",
                     new SqlParameter("@EmployeeId", id))
                 .ToListAsync();
        }

        public async Task UpdateOrderStatus(int orderId, int status)
        {
            var orderId_param = new SqlParameter("@OrderId", orderId);
            var statusId_param = new SqlParameter("@OrderStatus", status);
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.UpdateOrderStatus @OrderId, @OrderStatus", orderId_param, statusId_param);
        }
    }
}

