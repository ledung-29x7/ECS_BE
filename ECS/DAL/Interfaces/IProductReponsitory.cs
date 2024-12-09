using ECS.Areas.Client.Models;

namespace ECS.DAL.Interfaces
{
    public interface IProductReponsitory
    {
        Task<List<Product>> GetAllProduct() ;
        Task AddProduct(Product product);
    }
}
