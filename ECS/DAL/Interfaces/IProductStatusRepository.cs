using ECS.Areas.Client.Models;

namespace ECS.DAL.Interfaces
{
    public interface IProductStatusRepository
    {
        Task<List<ProductStatus>> GetAllProductStatus();
        Task<ProductStatus> GetProductStatusById(int statusId);
        Task AddProductStatus(ProductStatus productStatus);
        Task DeleteProductStatus(int statusId);
        Task UpdateProductStatus(ProductStatus productStatus);
    }
}
