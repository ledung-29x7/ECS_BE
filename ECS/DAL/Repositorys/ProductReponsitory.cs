using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace ECS.DAL.Repositorys
{
    public class ProductReponsitory : IProductReponsitory
    {
        private readonly ECSDbContext _context;
        private readonly IMapper _mapper;

        public ProductReponsitory(ECSDbContext eCSDbContext, IMapper mapper)
        {
            _context = eCSDbContext;
            _mapper = mapper;
        }




        public async Task DeleteProductAsync(Guid productId)
        {
            var param = new SqlParameter("@ProductId", productId);
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteProduct @ProductId", param);
        }

        public async Task<List<ProductWithImagesDTO>> GetAllProduct()
        {
            var products = new List<ProductWithImagesDTO>();
            var connection = _context.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetAllProduct";
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var productDictionary = new Dictionary<Guid, ProductWithImagesDTO>();

                    // Đọc thông tin sản phẩm
                    while (await reader.ReadAsync())
                    {
                        var productId = reader.GetGuid(reader.GetOrdinal("ProductId"));

                        if (!productDictionary.ContainsKey(productId))
                        {
                            var product = new Product
                            {
                                ProductId = productId,
                                ClientId = reader.GetGuid(reader.GetOrdinal("ClientId")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                InitialQuantity = reader.GetInt32(reader.GetOrdinal("InitialQuantity")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                Status = reader.GetInt32(reader.GetOrdinal("Status"))
                            };

                            var productDTO = _mapper.Map<ProductWithImagesDTO>(product);
                            productDTO.Images = new List<ImageTable>();

                            productDictionary.Add(productId, productDTO);
                        }
                    }

                    // Đọc thông tin ảnh
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var productId = reader.GetGuid(reader.GetOrdinal("ProductId"));
                            var image = new ImageTable
                            {
                                ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                                ImageBase64 = reader.GetString(reader.GetOrdinal("ImageBase64"))
                            };

                            if (productDictionary.ContainsKey(productId))
                            {
                                var imageDTO = _mapper.Map<ImageTable>(image);
                                productDictionary[productId].Images.Add(imageDTO);
                            }
                        }
                    }

                    products = productDictionary.Values.ToList();
                }
            }

            return products;
        }

        public async Task<ProductWithImagesDTO> GetProductById(Guid productId)
        {
            var productWithImagesDTO = new ProductWithImagesDTO();
            var connection = _context.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetProductById";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProductId", productId));

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Đọc thông tin sản phẩm
                    if (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                            ClientId = reader.GetGuid(reader.GetOrdinal("ClientId")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            InitialQuantity = reader.GetInt32(reader.GetOrdinal("InitialQuantity")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            Status = reader.GetInt32(reader.GetOrdinal("Status"))
                        };

                        // Ánh xạ Product sang ProductWithImagesDTO
                        productWithImagesDTO = _mapper.Map<ProductWithImagesDTO>(product);
                    }

                    // Đọc danh sách ảnh
                    if (await reader.NextResultAsync())
                    {
                        var images = new List<ImageTable>();

                        while (await reader.ReadAsync())
                        {
                            var image = new ImageTable
                            {
                                ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                                ImageBase64 = reader.GetString(reader.GetOrdinal("ImageBase64"))
                            };

                            images.Add(image);
                        }

                        // Ánh xạ danh sách ảnh sang DTO
                        productWithImagesDTO.Images = _mapper.Map<List<ImageTable>>(images);
                    }
                }
            }

            return productWithImagesDTO;
        }


        public async Task UpdateProductAsync(Product product)
        {
            var parameters = new[]
        {
            new SqlParameter("@ProductId", product.ProductId),
            new SqlParameter("@CategoryId", product.CategoryId),
            new SqlParameter("@ProductName", product.ProductName),
            new SqlParameter("@Price", product.Price),
            new SqlParameter("@InitialQuantity", product.InitialQuantity),
            new SqlParameter("@Description", product.Description)
        };

            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateProduct @ProductId, @CategoryId, @ProductName, @Price, @InitialQuantity, @Description", parameters);
        }

        public async Task<List<ProductWithImagesDTO>> GetProductsByClientIdAsync(Guid clientId)
        {
            var products = new List<ProductWithImagesDTO>();
            var connection = _context.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetProductsByClientId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ClientId", clientId));

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var productDictionary = new Dictionary<Guid, ProductWithImagesDTO>();

                    // Đọc thông tin sản phẩm
                    while (await reader.ReadAsync())
                    {
                        var productId = reader.GetGuid(reader.GetOrdinal("ProductId"));

                        if (!productDictionary.ContainsKey(productId))
                        {
                            var product = new Product
                            {
                                ProductId = productId,
                                ClientId = reader.GetGuid(reader.GetOrdinal("ClientId")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                InitialQuantity = reader.GetInt32(reader.GetOrdinal("InitialQuantity")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                Status = reader.GetInt32(reader.GetOrdinal("Status"))
                            };

                            var productDTO = _mapper.Map<ProductWithImagesDTO>(product);
                            productDTO.Images = new List<ImageTable>();

                            productDictionary.Add(productId, productDTO);
                        }
                    }

                    // Đọc thông tin ảnh
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var productId = reader.GetGuid(reader.GetOrdinal("ProductId"));
                            var image = new ImageTable
                            {
                                ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                                ImageBase64 = reader.GetString(reader.GetOrdinal("ImageBase64"))
                            };

                            if (productDictionary.ContainsKey(productId))
                            {
                                var imageDTO = _mapper.Map<ImageTable>(image);
                                productDictionary[productId].Images.Add(imageDTO);
                            }
                        }
                    }

                    products = productDictionary.Values.ToList();
                }
            }

            return products;
        }

        public async Task ActiveProduct(Guid productId)
        {
            var Id_Param = new SqlParameter("@ProductId", productId);
            await _context.Database.ExecuteSqlRawAsync("EXEC ActiveProduct @ProductId", Id_Param);
        }

        public async Task<Client> GetClientByProductId(Guid productId)
        {
            var ProductId_Param = new SqlParameter("@ProductId", productId);
            var clients = await _context.clients
              .FromSqlRaw("EXECUTE dbo.GetClientByProductId @ProductId", ProductId_Param)
              .ToListAsync();
            return clients.FirstOrDefault();
        }

        public async Task AddProduct(CreateProductRequest request, string productServicesJson)
        {
            // Deserialize ProductServices từ chuỗi JSON
            var productServices = new List<ProductServiceRequest>();
            if (!string.IsNullOrWhiteSpace(productServicesJson))
            {
                try
                {
                    productServices = JsonSerializer.Deserialize<List<ProductServiceRequest>>(productServicesJson);
                }
                catch (JsonException ex)
                {
                    throw new ArgumentException("Invalid ProductServices JSON format", ex);
                }
            }

            // Tạo DataTable cho Images
            var imagesTable = new DataTable();
            imagesTable.Columns.Add("ImageBase64", typeof(string));

            foreach (var imageFile in request.ImageFiles)
            {
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);
                var imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                imagesTable.Rows.Add(imageBase64);
            }

            // Tạo DataTable cho ProductServices
            var productServicesTable = new DataTable();
            productServicesTable.Columns.Add("ServiceId", typeof(int));
            productServicesTable.Columns.Add("ClientId", typeof(Guid));
            productServicesTable.Columns.Add("StartDate", typeof(DateTime));
            productServicesTable.Columns.Add("EndDate", typeof(DateTime));
            productServicesTable.Columns.Add("RequiredEmployees", typeof(int));

            if (productServices != null && productServices.Any())
            {
                foreach (var service in productServices)
                {
                    productServicesTable.Rows.Add(service.ServiceId, service.ClientId, service.StartDate, service.EndDate, service.RequiredEmployees);
                }
            }

            // Tham số cho Stored Procedure
            var parameters = new[]
            {
        new SqlParameter("@ClientId", request.ClientId),
        new SqlParameter("@CategoryId", request.CategoryId),
        new SqlParameter("@ProductName", request.ProductName),
        new SqlParameter("@Price", request.Price),
        new SqlParameter("@InitialQuantity", request.InitialQuantity),
        new SqlParameter("@Description", request.Description),
        new SqlParameter("@Images", imagesTable)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.ImageTableType"
        },
        new SqlParameter("@ProductServices", productServicesTable)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.ProductServiceType"
        }
    };

            // Gọi Stored Procedure
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddProductWithImagesAndServices @ClientId, @CategoryId, @ProductName, @Price, @InitialQuantity, @Description, @Images, @ProductServices",
                parameters
            );
        }


        public async Task AddProductWithImageAsync(Product product, List<ImageTable> images)
        {
            var imageDataTable = new DataTable();
            imageDataTable.Columns.Add("ImageBase64", typeof(string));

            foreach (var image in images)
            {
                imageDataTable.Rows.Add(image.ImageBase64);
            }

            var ClientId_Param = new SqlParameter("@ClientId", product.ClientId);
            var CategoryId_Param = new SqlParameter("@CategoryId", product.CategoryId);
            var productNameParam = new SqlParameter("@ProductName", product.ProductName);
            var priceParam = new SqlParameter("@Price", product.Price);
            var initialQuantityParam = new SqlParameter("@InitialQuantity", product.InitialQuantity);
            var descriptionParam = new SqlParameter("@Description", product.Description ?? (object)DBNull.Value);

            var imagesParam = new SqlParameter("@Images", SqlDbType.Structured)
            {
                TypeName = "dbo.ImageTableType",
                Value = imageDataTable
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddProductWithImages @ClientId, @CategoryId, @ProductName, @Price, @InitialQuantity, @Description, @Images",
                ClientId_Param, CategoryId_Param, productNameParam, priceParam, initialQuantityParam, descriptionParam, imagesParam
            );
        }

        public async Task<(IEnumerable<ProductDto> Products, int TotalRecords, int TotalPages)> GetAllProductsAsync(
    int pageNumber,
    string searchTerm = null,
    decimal? priceFilter = null)
        {
            var productDictionary = new Dictionary<Guid, ProductDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var searchTermParam = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);
            var priceFilterParam = new SqlParameter("@PriceFilter", priceFilter ?? (object)DBNull.Value);

            var results = await _context.Set<RawProductResult>()
                .FromSqlRaw(
                    "EXEC [dbo].[GetAllProduct] @PageNumber, @SearchTerm, @PriceFilter",
                    pageNumberParam,
                    searchTermParam,
                    priceFilterParam
                )
                .ToListAsync();

            int totalRecords = results.FirstOrDefault()?.TotalRecords ?? 0;
            int totalPages = results.FirstOrDefault()?.TotalPages ?? 0;

            foreach (var result in results)
            {
                if (!productDictionary.TryGetValue(result.ProductId, out var product))
                {
                    product = new ProductDto
                    {
                        ProductId = result.ProductId,
                        ClientId = result.ClientId,
                        CategoryId = result.CategoryId,
                        ProductName = result.ProductName,
                        Price = result.Price,
                        InitialQuantity = result.InitialQuantity,
                        Description = result.Description,
                        IsActive = result.IsActive,
                        Status = result.Status,
                        CreatedAt = result.CreatedAt,
                        Images = new List<ImageTable>()
                    };

                    productDictionary.Add(result.ProductId, product);
                }

                if (result.ImageId.HasValue && !string.IsNullOrEmpty(result.ImageBase64))
                {
                    product.Images.Add(new ImageTable
                    {
                        ImageId = result.ImageId.Value,
                        ImageBase64 = result.ImageBase64
                    });
                }
            }

            return (productDictionary.Values, totalRecords, totalPages);
        }
    }
}
