using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ProductCategoryReponsitory : IProductCategoryReponsitory
    {
        private readonly ECSDbContext eCSDbContext;

        public ProductCategoryReponsitory(ECSDbContext eCSDbContext)
        {
            this.eCSDbContext = eCSDbContext;
        }
        public async Task<List<ProductCategory>> GetAllCategory()
        {

            return await eCSDbContext.ProductCategory
                .FromSqlRaw("EXEC dbo.GetAllProductCategories")
                .ToListAsync();

        }
        public async Task DeleteProductCategory(int id)
        {
            var productid_param = new SqlParameter("@CategoryId", id);
            await eCSDbContext.Database.ExecuteSqlRawAsync("EXEC DeleteProductCategory @CategoryId", productid_param);
        }
        public async Task<ProductCategory> GetProductCategorybyID(int id)
        {

            var product = await eCSDbContext.ProductCategory
                .FromSqlRaw("EXEC GetProductCategoryById @CategoryId", new SqlParameter("@CategoryId", id))
                .ToListAsync();
            return product.FirstOrDefault();

        }


        public async Task AddProductCategory(ProductCategory productCategory)
        {
            try
            {
                var categoryName_param = new SqlParameter("@CategoryName", productCategory.CategoryName);
                await eCSDbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.AddProductCategory  @CategoryName",
                    categoryName_param);
                 

            }
            catch (Exception ex)
            {

                throw new Exception("Error adding product: " + ex.Message, ex);
            }
        }

        public async Task UpdateProductCategory(ProductCategory productCategory)
        {
            try
            {
                var categoryIdParam = new SqlParameter("@CategoryId", productCategory.CategoryId);
                var categoryNameParam = new SqlParameter("@CategoryName", productCategory.CategoryName);

                await eCSDbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.UpdateProductCategory @CategoryId, @CategoryName",
                    categoryIdParam,
                    categoryNameParam
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product category: " + ex.Message, ex);
            }
        }

    }
}
