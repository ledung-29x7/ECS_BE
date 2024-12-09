using ECS.Areas.Units.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ECS.DAL.Interfaces
{
    public interface IImageRepository
    {
        Task AddImages(List<ImageTable> images);
        Task<List<ImageTable>> GetImageByEmloyeeId(Guid employeeId);
        Task<List<ImageTable>> GetImageByProductId(Guid productId);

        Task DeleteImageByEmployeeId(Guid employeeId);
        Task DeleteImageByProductId(Guid productId);
        public Task AddImagesToProduct(Guid productId, List<ImageTable> images);
        public Task AddImagesToEmployee(Guid employeeId, List<ImageTable> images);
    }
}
