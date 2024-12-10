using ECS.Areas.Client.Models;

namespace ECS.DAL.Interfaces
{
    public interface IProductServiceRepository
    {
        Task CreateProductService(ProductService productService);
        Task<List<ProductService>> GetAllProductService();
        Task UpdateProductService (ProductService productService);
        Task<List<ProductService>> GetProductServiceByClientId(Guid clientId);
        
    }
}
