using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECS.DAL.Repositorys
{
    public class ImageRepository : IImageRepository
    {
        private readonly ECSDbContext _context;

        public ImageRepository(ECSDbContext context) 
        {
            _context = context;
        }
        public async Task AddImages(List<ImageTable> images)
        {
            var table = new DataTable();
            table.Columns.Add("ImageBase64", typeof(string));

            foreach (var image in images)
            {
                table.Rows.Add(image.ImageBase64);
            }
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddImages @ImageTable",
                new[] {
                    new SqlParameter("@ImageTable", table)
                    {
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "ImageTableType"
                    }
                });
        }

        public async Task AddImagesToEmployee(Guid employeeId, List<ImageTable> images)
        {
            var table = new DataTable();
            table.Columns.Add("ImageBase64", typeof(string));

            foreach (var image in images)
            {
                table.Rows.Add(image.ImageBase64);
            }

            // Gọi stored procedure
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddEmployeeImages @EmployeeId, @ImageTable",
                new[]
                {
            new SqlParameter("@EmployeeId", employeeId),
            new SqlParameter("@ImageTable", table)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "ImageTableType"
            }
                });
        }

        public async Task AddImagesToProduct(Guid productId, List<ImageTable> images)
        {
            var table = new DataTable();
            table.Columns.Add("ImageBase64", typeof(string));

            foreach (var image in images)
            {
                table.Rows.Add(image.ImageBase64);
            }
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddProductImages @ProductId, @ImageTable",
                new[]
                {
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@ImageTable", table)
                    {
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "ImageTableType"
                    }
                });
        }

        public async Task DeleteImageByEmployeeId(Guid employeeId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC DeleteImagesByEmployeeId @EmployeeId",
                new[] {
                    new SqlParameter("@EmployeeId", employeeId)
                });
        }

        public async Task DeleteImageByProductId(Guid productId)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteImagesByProductId @ProductId",new[] {
                    new SqlParameter("@ProductId", productId)
            });
        }

        public async Task<List<ImageTable>> GetImageByEmloyeeId(Guid employeeId)
        {
            return await _context.imageTables
                .FromSqlRaw("EXEC GetImagesByEmployeeId @EmployeeId", new[] {
                    new SqlParameter("@EmployeeId", employeeId)
                })
                .ToListAsync();
        }

        public async Task<List<ImageTable>> GetImageByProductId(Guid productId)
        {
            return await _context.imageTables
                .FromSqlRaw("EXEC GetImagesByProductId @ProductId", new[] {
                    new SqlParameter("@ProductId", productId)
                })
                .ToListAsync();
        }
    }
}
