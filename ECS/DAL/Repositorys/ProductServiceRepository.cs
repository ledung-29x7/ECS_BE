using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ProductServiceRepository : IProductServiceRepository
    {
        private readonly ECSDbContext _context;

        public ProductServiceRepository(ECSDbContext context) 
        {
            _context= context;
        }
        public async Task CreateProductService(ProductService productService)
        {
            var ServiceId_Param = new SqlParameter("@ServiceId", productService.ServiceId);
            var ProductId_Param = new SqlParameter("@ProductId", productService.ProductId);
            var ClientId_Param = new SqlParameter("@ClientId", productService.ClientId);
            var StartDate_Param = new SqlParameter("@StartDate", productService.StartDate);
            var EndDate_Param = new SqlParameter("@EndDate", productService.EndDate);
            var RequiredEmployees_Param = new SqlParameter("@RequiredEmployees", productService.RequiredEmployees);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE CreateProductService @ServiceId, @ProductId, @ClientId, @StartDate, @EndDate, @RequiredEmployees", ServiceId_Param, ProductId_Param, ClientId_Param, StartDate_Param, EndDate_Param, RequiredEmployees_Param);
        }

        public async Task<List<ProductService>> GetAllProductService()
        {
            return await Task.FromResult(_context.productServices.FromSqlRaw("EXECUTE dbo.GetAllProductService").ToList());
        }

        public async Task<List<ProductService>> GetProductServiceByClientId(Guid clientId)
        {
            var ClientId_Param = new SqlParameter("@ClientId", clientId);
            var productServices = await _context.productServices
                                       .FromSqlRaw("EXECUTE dbo.GetProductServiceByClientId @ClientId", ClientId_Param)
                                       .ToListAsync();
            return productServices;
        }

        public async Task<List<ProductService>> GetProductServiceByProductId(Guid productId)
        {
            var ProductId_Param = new SqlParameter("@ProductId", productId);
            var productServices = await _context.productServices.FromSqlRaw("EXECUTE dbo.GetProductServiceByProductId @ProductId", ProductId_Param)
                                       .ToListAsync();
            return productServices;
        }

        public async Task UpdateProductService(ProductService productService)
        {
            var ProductServiceId_Param = new SqlParameter("@ProductServiceId", productService.ProductServiceId);
            var ServiceId_Param = new SqlParameter("@ServiceId", productService.ServiceId);
            var ProductId_Param = new SqlParameter("@ProductId", productService.ProductId);
            var ClientId_Param = new SqlParameter("@ClientId", productService.ClientId);
            var StartDate_Param = new SqlParameter("@StartDate", productService.StartDate);
            var EndDate_Param = new SqlParameter("@EndDate", productService.EndDate);
            var RequiredEmployees_Param = new SqlParameter("@RequiredEmployees", productService.RequiredEmployees);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE UpdateProductService @ProductServiceId ,@ServiceId, @ProductId, @ClientId, @StartDate, @EndDate, @RequiredEmployees", ProductServiceId_Param, ServiceId_Param, ProductId_Param, ClientId_Param, StartDate_Param, EndDate_Param, RequiredEmployees_Param);
        }
    }
}
