using System.Data;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ProductSalesRepository : IProductSalesRepository
    {
        private readonly ECSDbContext _dbContext;

        public ProductSalesRepository(ECSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductReportDto>> GetProductSalesAndStockAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var reports = new List<ProductReportDto>();

            using (var connection = _dbContext.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetProductSalesAndStock";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = (object?)startDate ?? DBNull.Value });
                    command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = (object?)endDate ?? DBNull.Value });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var stockAvailable = reader.GetInt32(reader.GetOrdinal("StockAvailable"));
                            var stockStatus = reader.GetString(reader.GetOrdinal("StockStatus"));

                            reports.Add(new ProductReportDto
                            {
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                TotalSold = reader.GetInt32(reader.GetOrdinal("TotalSold")),
                                TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                StockAvailable = stockAvailable,
                                StockStatus = stockStatus  
                            });
                        }
                    }
                }
            }

            return reports;
        }

    }
}
