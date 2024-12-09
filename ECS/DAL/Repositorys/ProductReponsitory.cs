using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ProductReponsitory : IProductReponsitory
    {
        private readonly ECSDbContext eCSDbContext;

        public ProductReponsitory(ECSDbContext eCSDbContext)
        {
            this.eCSDbContext = eCSDbContext;
        }
        public async Task<List<Product>> GetAllProduct()
        {
     
                return await eCSDbContext.product
                    .FromSqlRaw("EXEC dbo.GetProduct")
                    .ToListAsync();
    
        }

        public async Task AddProduct(Product product)
        {
            try
            {
                var client_Param = new SqlParameter("@ClientId", product.ClientId);
                var productName_param = new SqlParameter("@ProductName", product.ProductName);
                var categoryid_Param = new SqlParameter("@CategoryId", product.CategoryId);
                var price_Param = new SqlParameter("@Price", product.Price);
                var quantity_Param = new SqlParameter("@InitialQuantity", product.InitialQuantity);
                var description_Param = new SqlParameter("@Description", product.Description);
                var active_Param = new SqlParameter("@IsActive", product.IsActive);
                var status_Param = new SqlParameter("@Status", product.Status);

                await eCSDbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.AddProduct @ClientId, @CategoryId, @ProductName, @Price, @InitialQuantity, @Description, @IsActive, @Status",
                    client_Param,
                    categoryid_Param,
                    productName_param,
                    price_Param,
                    quantity_Param,
                    description_Param,
                    active_Param,
                    status_Param);

            }
            catch (Exception ex)
            {
               
                throw new Exception("Error adding product: " + ex.Message, ex);
            }
        }

    }
}
