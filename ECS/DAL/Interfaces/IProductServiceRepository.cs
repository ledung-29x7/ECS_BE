using ECS.Areas.Client.Models;
using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IProductServiceRepository
    {
        Task CreateProductService(ProductService productService);
        //Task<List<ProductService>> GetAllProductService();
        Task UpdateProductService (ProductService productService);
        Task<List<ProductServiceDto>> GetProductServiceByClientId(Guid clientId);
        Task<List<ProductService>> GetProductServiceByProductId(Guid productId);

        Task<(List<ProductService> ProductServices, int TotalRecords, int TotalPages)> GetAllProductService(int pageNumber);


    }
}
