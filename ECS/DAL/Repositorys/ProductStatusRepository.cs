using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ProductStatusRepository : IProductStatusRepository
    {
        private readonly ECSDbContext _context;
        public ProductStatusRepository(ECSDbContext context) 
        {
            _context = context;
        }
        public async Task AddProductStatus(ProductStatus productStatus)
        {
            var Name_Param = new SqlParameter("@StatusName", productStatus.StatusName);
            await _context.Database.ExecuteSqlRawAsync("EXEC AddProductStatus @StatusName", Name_Param);

        }

        public async Task DeleteProductStatus(int statusId)
        {
            var id_Param = new SqlParameter("@StatusId", statusId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteProductStatus @StatusId", id_Param);
        }

        public async Task<List<ProductStatus>> GetAllProductStatus()
        {
            return await Task.FromResult(_context.productStatuses.FromSqlRaw("EXECUTE dbo.GetAllProductStatus").ToList());

        }

        public async Task<ProductStatus> GetProductStatusById(int statusId)
        {
            var id_param = new SqlParameter("@StatusId", statusId);
            var productStatuses = await _context.productStatuses
               .FromSqlRaw("EXECUTE dbo.GetProductStatusById @StatusId", id_param)
               .ToListAsync();
            return productStatuses.FirstOrDefault();
        }

        public async Task UpdateProductStatus(ProductStatus productStatus)
        {
            var id_param = new SqlParameter("@StatusId", productStatus.StatusId);
            var name_param = new SqlParameter("@StatusName", productStatus.StatusName);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateProductStatus @StatusId, @StatusName", id_param, name_param);
        }
    }
}
