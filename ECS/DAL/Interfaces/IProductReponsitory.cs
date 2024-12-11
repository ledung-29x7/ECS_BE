using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IProductReponsitory
    {
        Task<List<Product>> GetAllProduct() ;
        Task<ProductWithImagesDTO> GetProductById(Guid productId);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
        Task AddProductWithImageAsync(Product product, List<ImageTable> images);
        Task<List<ProductWithImagesDTO>> GetProductsByClientIdAsync(Guid clientId);
    }
}
