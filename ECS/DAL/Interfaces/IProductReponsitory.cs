using ECS.Areas.Client.Models;

namespace ECS.DAL.Interfaces
{
    public interface IProductReponsitory
    {
        Task<List<Product>> GetAllProduct() ;
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
       Task<Product>  GetProductbyID(Guid id);

        Task DeleteProduct(Guid id);
    }
}
