using ECS.Areas.Admin.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IProductReponsitory
    {
        Task<List<ProductWithImagesDTO>> GetAllProduct() ;
        Task<ProductWithImagesDTO> GetProductById(Guid productId);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
        Task AddProductWithImageAsync(Product product, List<ImageTable> images);
        Task<(IEnumerable<ProductDto> Products, int TotalRecords, int TotalPages)> GetProductsByClientIdAsync(
                Guid clientId,
                int pageNumber = 1,
                string searchTerm = null,
                bool? isActive = null);
        Task ActiveProduct(Guid productId);

        Task<Client> GetClientByProductId(Guid productId);
        Task AddProduct(CreateProductRequest request, string productServicesJson);
       Task<(IEnumerable<ProductDto> Products, int TotalRecords, int TotalPages)> GetAllProductsAsync(
            int pageNumber,
            string searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null);

        Task<(IEnumerable<ProductByClientIdDto> Products, int TotalRecords, int TotalPages)> GetProductsByClientIdWithSalesAsync(
            Guid clientId,
            int pageNumber = 1,
            string searchTerm = null,
            bool? isActive = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
    }

}
