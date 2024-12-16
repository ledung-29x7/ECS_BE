using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IProductSalesRepository
    {
        public  Task<List<ProductReportDto>> GetProductSalesAndStockAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
