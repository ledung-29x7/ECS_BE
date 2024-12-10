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
        public async Task UpdateProductActivation(Guid productId, bool isActive)
        {
            try
            {
                var productIdParam = new SqlParameter("@ProductId", productId);
                var isActiveParam = new SqlParameter("@IsActive", isActive);

                await eCSDbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.UpdateProductActivation @ProductId, @IsActive",
                    productIdParam,
                    isActiveParam
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product activation status: " + ex.Message, ex);
            }
        }
        public async Task<List<Product>> GetAllProduct()
        {
     
                return await eCSDbContext.product
                    .FromSqlRaw("EXEC dbo.GetProduct")
                    .ToListAsync();
    
        }
        public async Task DeleteProduct(Guid id)
        {
            var productid_param = new SqlParameter("@ProductId", id);
           await eCSDbContext.Database.ExecuteSqlRawAsync("EXEC GetProductById @ProductId" , productid_param); 
        }
        public async Task<Product> GetProductbyID(Guid id)
        {

            var product = await eCSDbContext.product
                .FromSqlRaw("EXEC GetProductById @ProductId", new SqlParameter("@ProductId", id))
                .ToListAsync();
            return product.FirstOrDefault();
            
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

        public async Task UpdateProduct(Product product)
        {
            try
            {
                var productIdParam = new SqlParameter("@ProductId", product.ProductId);
                var clientIdParam = new SqlParameter("@ClientId", product.ClientId);
                var categoryIdParam = new SqlParameter("@CategoryId", product.CategoryId);
                var productNameParam = new SqlParameter("@ProductName", product.ProductName);
                var priceParam = new SqlParameter("@Price", product.Price);
                var initialQuantityParam = new SqlParameter("@InitialQuantity", product.InitialQuantity);
                var descriptionParam = new SqlParameter("@Description", product.Description);
                var isActiveParam = new SqlParameter("@IsActive", product.IsActive);
                var statusParam = new SqlParameter("@Status", product.Status);

                await eCSDbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.UpdateProduct @ProductId, @ClientId, @CategoryId, @ProductName, @Price, @InitialQuantity, @Description, @IsActive, @Status",
                    productIdParam,
                    clientIdParam,
                    categoryIdParam,
                    productNameParam,
                    priceParam,
                    initialQuantityParam,
                    descriptionParam,
                    isActiveParam,
                    statusParam
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product: " + ex.Message, ex);
            }
        }

    }
}
