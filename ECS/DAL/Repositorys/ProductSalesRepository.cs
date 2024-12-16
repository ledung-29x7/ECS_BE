using System.Data;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ProductSalesRepository : IProductSalesRepository
    {
        private readonly ECSDbContext eCSDbContext;

        public ProductSalesRepository(ECSDbContext eCSDbContext)
        {
            this.eCSDbContext = eCSDbContext;
        }
        public async Task<List<ProductReportDto>> GetProductSalesAndStockAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var reports = new List<ProductReportDto>();

            using (var connection = eCSDbContext.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetProductSalesAndStock";
                    command.CommandType = CommandType.StoredProcedure;

                    var startParam = new SqlParameter("@StartDate", SqlDbType.Date)
                    {
                        Value = (object?)startDate ?? DBNull.Value
                    };
                    command.Parameters.Add(startParam);

                    var endParam = new SqlParameter("@EndDate", SqlDbType.Date)
                    {
                        Value = (object?)endDate ?? DBNull.Value
                    };
                    command.Parameters.Add(endParam);

                    using (var reader =  await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reports.Add(new ProductReportDto
                            {
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                TotalSold = reader.GetInt32(reader.GetOrdinal("TotalSold")),
                                TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                StockAvailable = reader.GetInt32(reader.GetOrdinal("StockAvailable"))
                            });
                        }
                    }
                }
            }

            return reports;
        }
    }
}
