using ECS.Areas.Client.Models;

namespace ECS.DAL.Interfaces
{
    public interface IProductCategoryReponsitory
    {
        Task<List<ProductCategory>> GetAllCategory();
        Task AddProductCategory(ProductCategory productCategory);
        Task UpdateProductCategory(ProductCategory productCategory);
        Task<ProductCategory> GetProductCategorybyID(int id);

        Task DeleteProductCategory(int  id);
    }
}
