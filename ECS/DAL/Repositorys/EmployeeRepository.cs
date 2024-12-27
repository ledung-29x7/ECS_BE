using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using ECS.Dtos;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECS.DAL.Repositorys
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ECSDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(ECSDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteEmployee(Guid employeeid)
        {
            var id_param = new SqlParameter("@EmployeeId", employeeid);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteEmployee @RoleId", id_param);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            var email_Param = new SqlParameter("@Email", email);
            var users = await _context.employees.FromSqlRaw("EXEC GetEmployeeByEmail @Email", email_Param).ToListAsync();
            return users.FirstOrDefault();
        }

        public async Task RegisterEmployee(Employee employee)
        {

            var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
            var LastName_param = new SqlParameter("@LastName", employee.LastName);
            var email_param = new SqlParameter("@Email", employee.Email);
            var Phone_number_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
            var Password_param = new SqlParameter("@Password", employee.Password);
            await _context.Database.ExecuteSqlRawAsync("EXEC RegisterEmployee @FirstName, @LastName, @Email, @PhoneNumber, @Password", FirstName_param, LastName_param, email_param, Phone_number_param, Password_param);
            
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var id_param = new SqlParameter("@EmployeeId", employee.EmployeeId);
            var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
            var LastName_param = new SqlParameter("@LastName", employee.LastName);
            var Email_param = new SqlParameter("@Email", employee.Email);
            var PhoneNumber_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateEmployee @EmployeeId, @FirstName, @LastName, @Email, @PhoneNumber", id_param, FirstName_param, LastName_param, Email_param, PhoneNumber_param);
        }

        public async Task UpdateEmployeeRole(Guid EmployeeId, int RoleId)
        {
            var id_param = new SqlParameter("@EmployeeId", EmployeeId);
            var RoleId_param = new SqlParameter("@RoleId", RoleId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateEmployeeRole @EmployeeId, @RoleId ", id_param, RoleId_param );
        }

        public async Task UpdateDepartmentForEmployee(Guid EmployeeId, int departmentsId)
        {
            var EmployeeId_Param = new SqlParameter("@EmployeeId", EmployeeId);
            var departmentsId_Param = new SqlParameter("@DepartmentsId", departmentsId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateDepartmentForEmployee @EmployeeId, @DepartmentsId", EmployeeId_Param, departmentsId_Param);
        }

        public async Task DeleteEmployeeAndUnsetManager(Guid EmployeeId)
        {
            var EmployeeId_Param = new SqlParameter("@EmployeeId", EmployeeId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteEmployeeAndUnsetManager @EmployeeId", EmployeeId_Param);
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            return await Task.FromResult(_context.employees.FromSqlRaw("EXECUTE dbo.GetAllEmployee").ToList());
        }

        //public async Task AddEmployeeWithImagesAsync(Employee employee, List<ImageTable> images)
        //{
        //    var imageDataTable = new DataTable();
        //    imageDataTable.Columns.Add("ImageBase64", typeof(string));

        //    foreach (var image in images)
        //    {
        //        imageDataTable.Rows.Add(image.ImageBase64);
        //    }
        //    var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
        //    var LastName_param = new SqlParameter("@LastName", employee.LastName);
        //    var email_param = new SqlParameter("@Email", employee.Email);
        //    var DepartmentID_param = new SqlParameter("@DepartmentID", employee.DepartmentID);
        //    var RoleId_param = new SqlParameter("@RoleId", employee.RoleId);
        //    var Phone_number_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
        //    var Password_param = new SqlParameter("@Password", employee.Password);
        //    var imagesParam = new SqlParameter("@Images", SqlDbType.Structured)
        //    {
        //        TypeName = "dbo.ImageTableType",
        //        Value = imageDataTable
        //    };
        //    await _context.Database.ExecuteSqlRawAsync(
        //        "EXEC AddEmployeeWithImages @FirstName, @LastName, @Email,@DepartmentID, @RoleId, @PhoneNumber, @Password, @Images",
        //        FirstName_param, LastName_param, email_param,DepartmentID_param, RoleId_param, Phone_number_param, Password_param, imagesParam
        //    );
        //}
        public async Task<Guid> AddEmployeeWithImagesAsync(Employee employee, List<ImageTable> images, List<int> categoryIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Add Employee and get EmployeeId
                var imageDataTable = new DataTable();
                imageDataTable.Columns.Add("ImageBase64", typeof(string));

                foreach (var image in images)
                {
                    imageDataTable.Rows.Add(image.ImageBase64);
                }

                var employeeIdParam = new SqlParameter("@EmployeeId", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };

                var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
                var LastName_param = new SqlParameter("@LastName", employee.LastName);
                var email_param = new SqlParameter("@Email", employee.Email);
                var DepartmentID_param = new SqlParameter("@DepartmentID", employee.DepartmentID);
                var RoleId_param = new SqlParameter("@RoleId", employee.RoleId);
                var Phone_number_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
                var Password_param = new SqlParameter("@Password", employee.Password);
                var imagesParam = new SqlParameter("@Images", SqlDbType.Structured)
                {
                    TypeName = "dbo.ImageTableType",
                    Value = imageDataTable
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddEmployeeWithImages @FirstName, @LastName, @Email, @DepartmentID, @RoleId, @PhoneNumber, @Password, @Images, @EmployeeId OUTPUT",
                    FirstName_param, LastName_param, email_param, DepartmentID_param, RoleId_param, Phone_number_param, Password_param, imagesParam, employeeIdParam
                );

                var employeeId = (Guid)employeeIdParam.Value;

                // 2. Add EmployeeProductCategory
                foreach (var categoryId in categoryIds)
                {
                    var employeeIdCategoryParam = new SqlParameter("@EmployeeId", employeeId);
                    var categoryIdParam = new SqlParameter("@CategoryId", categoryId);

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC AddEmployeeProductCategory @EmployeeId, @CategoryId",
                        employeeIdCategoryParam, categoryIdParam
                    );
                }

                // 3. Commit transaction
                await transaction.CommitAsync();

                return employeeId;
            }
            catch
            {
                // Rollback transaction in case of error
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<EmployeeWithImagesDTO>> GetAllEmployeesAsync()
        {
            var employees = new List<EmployeeWithImagesDTO>();
            var connection = _context.Database.GetDbConnection();


            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetAllEmployee";
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var employeeDictionary = new Dictionary<Guid, EmployeeWithImagesDTO>();

                    // Đọc thông tin sản phẩm
                    while (await reader.ReadAsync())
                    {
                        var employeeId = reader.GetGuid(reader.GetOrdinal("EmployeeId"));

                        if (!employeeDictionary.ContainsKey(employeeId))
                        {
                            var employee = new EmployeeWithImagesDTO
                            {
                                EmployeeId = reader.IsDBNull(reader.GetOrdinal("EmployeeId")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoleId")),
                                DepartmentId = reader.IsDBNull(reader.GetOrdinal("DepartmentID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                Images = new List<ImageTable>()
                            };
                            employeeDictionary.Add(employeeId, employee);
                        }
                    }

                    // Đọc thông tin ảnh
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var employeeId = reader.GetGuid(reader.GetOrdinal("EmployeeId"));
                            var image = new ImageTable
                            {
                                ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                                ImageBase64 = reader.GetString(reader.GetOrdinal("ImageBase64"))
                            };

                            if (employeeDictionary.ContainsKey(employeeId))
                            {
                                var imageDTO = _mapper.Map<ImageTable>(image);
                                employeeDictionary[employeeId].Images.Add(imageDTO);
                            }
                        }
                    }

                    employees = employeeDictionary.Values.ToList();
                }
            }

            return employees;
        }

        public async Task ChangePassword(Guid employeeId, string oldPasswordHash, string newPasswordHash)
        {
            var EmployeeId_Param = new SqlParameter("@EmployeeId", employeeId);
            var employees = await _context.employees.FromSqlRaw("EXEC GetEmployeeById @EmployeeId", EmployeeId_Param).ToListAsync();
            var employee = employees.FirstOrDefault();
            if (employee == null)
            {
                throw new Exception("User not found.");
            }

            // So sánh mật khẩu người dùng nhập vào với mật khẩu đã băm trong cơ sở dữ liệu
            if (!BCrypt.Net.BCrypt.Verify(oldPasswordHash, employee.Password))
            {
                throw new Exception("Old password does not match.");
            }

            // Nếu mật khẩu cũ khớp, gọi stored procedure để cập nhật mật khẩu mới
            var id_param = new SqlParameter("@EmployeeId", employeeId);
            var newPasswordHash_param = new SqlParameter("@NewPasswordHash", newPasswordHash);

            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.ChangePasswordEmployee @EmployeeId, @NewPasswordHash", id_param, newPasswordHash_param);
        }
        public async Task<List<EmployeeWorkListDto>> GetEmployeeWorkListByIdAsync(Guid employeeId)
        {
            var employeeWorkList = await _context.employeeWorkListDTOs
                .FromSqlInterpolated($@"
                    EXEC GetEmployeeWorkListById @EmployeeId = {employeeId}
                ")
                .ToListAsync();

            return employeeWorkList;
        }

        public async Task<List<EmployeeAvailable>> GetEmployeeAvailables(Guid productId, int RequiredEmployees)
        {
            var productId_Param = new SqlParameter("@ProductId", productId);
            var RequiredEmployees_param = new SqlParameter("@RequiredEmployees", RequiredEmployees);
            var employees = await Task.FromResult(_context.employeeAvailables.FromSqlRaw("EXECUTE dbo.GetAvailableEmployees @ProductId, @RequiredEmployees", productId_Param, RequiredEmployees_param).ToList());
            return employees;
        }

        public async Task<Employee> GetEmployeeById(Guid employeeId)
        {
            var employeeId_Param = new SqlParameter("@EmployeeId", employeeId);
            var employee  = await _context.employees
               .FromSqlRaw("EXECUTE dbo.GetEmployeeById @EmployeeId", employeeId_Param)
               .ToListAsync();
            return employee.FirstOrDefault();
        }

        public async Task<(IEnumerable<EmployeeDto> Employees, int TotalRecords, int TotalPages)> GetAllEmployeeAndSearchAsync(int pageNumber, string searchTerm)
        {
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var searchTermParam = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);

            var result = await _context.employeeDtos
                .FromSqlInterpolated($"EXEC GetAllEmployee @PageNumber = {pageNumberParam}, @SearchTerm = {searchTermParam}")
                .ToListAsync();

            // Extract metadata from the first item (assuming all items have the same TotalRecords and TotalPages).
            int totalRecords = result.FirstOrDefault()?.TotalRecords ?? 0;
            int totalPages = result.FirstOrDefault()?.TotalPages ?? 0;

            return (result, totalRecords, totalPages);
        }

        //     public async Task<(IEnumerable<EmployeeDto> Employees, int TotalRecords, int TotalPages)> GetAllEmployeesAsync(
        //int pageNumber,
        //string searchTerm)
        //     {
        //         var employeeDictionary = new Dictionary<Guid, EmployeeDto>();

        //         // Define output parameters for TotalRecords and TotalPages
        //         var totalRecordsParam = new SqlParameter
        //         {
        //             ParameterName = "@TotalRecords",
        //             SqlDbType = System.Data.SqlDbType.Int,
        //             Direction = System.Data.ParameterDirection.Output
        //         };

        //         var totalPagesParam = new SqlParameter
        //         {
        //             ParameterName = "@TotalPages",
        //             SqlDbType = System.Data.SqlDbType.Int,
        //             Direction = System.Data.ParameterDirection.Output
        //         };

        //         // Input parameters
        //         var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        //         var searchTermParam = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);

        //         // Execute stored procedure
        //         var results = await _context.Set<RawEmployeeResult>()
        //             .FromSqlRaw(
        //                 "EXEC [dbo].[GetAllEmployee] @PageNumber, @SearchTerm, @TotalRecords OUTPUT, @TotalPages OUTPUT",
        //                 pageNumberParam,
        //                 searchTermParam,
        //                 totalRecordsParam,
        //                 totalPagesParam
        //             )
        //             .ToListAsync();

        //         // Process the raw results into the dictionary
        //         foreach (var result in results)
        //         {
        //             if (!employeeDictionary.TryGetValue(result.EmployeeId, out var employee))
        //             {
        //                 employee = new EmployeeDto
        //                 {
        //                     EmployeeId = result.EmployeeId,
        //                     FirstName = result.FirstName,
        //                     LastName = result.LastName,
        //                     Email = result.Email,
        //                     PhoneNumber = result.PhoneNumber,
        //                     RoleId = result.RoleId,
        //                     DepartmentID = result.DepartmentID,
        //                     Images = new List<ImageTable>(),
        //                     Categories = new List<ProductCategory>()
        //                 };

        //                 employeeDictionary.Add(result.EmployeeId, employee);
        //             }

        //             if (result.ImageId.HasValue && !string.IsNullOrEmpty(result.ImageBase64))
        //             {
        //                 employee.Images.Add(new ImageTable
        //                 {
        //                     ImageId = result.ImageId.Value,
        //                     ImageBase64 = result.ImageBase64
        //                 });
        //             }
        //         }

        //         // Retrieve output parameter values
        //         int totalRecords = (int)(totalRecordsParam.Value ?? 0);
        //         int totalPages = (int)(totalPagesParam.Value ?? 0);

        //         return (employeeDictionary.Values, totalRecords, totalPages);
        //     }
        public async Task<(IEnumerable<EmployeeDto> Employees, int TotalRecords, int TotalPages)> GetAllEmployeesAsync(
         int pageNumber,
         string searchTerm)
        {
            var employeeDictionary = new Dictionary<Guid, EmployeeDto>();

            // Define output parameters for TotalRecords and TotalPages
            var totalRecordsParam = new SqlParameter
            {
                ParameterName = "@TotalRecords",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var totalPagesParam = new SqlParameter
            {
                ParameterName = "@TotalPages",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Input parameters
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var searchTermParam = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);

            // Execute stored procedure
            var results = await _context.Set<RawEmployeeResult>()
                .FromSqlRaw(
                    "EXEC [dbo].[GetAllEmployee] @PageNumber, @SearchTerm, @TotalRecords OUTPUT, @TotalPages OUTPUT",
                    pageNumberParam,
                    searchTermParam,
                    totalRecordsParam,
                    totalPagesParam
                )
                .ToListAsync();

            // Process the raw results into the dictionary
            foreach (var result in results)
            {
                if (!employeeDictionary.TryGetValue(result.EmployeeId, out var employee))
                {
                    employee = new EmployeeDto
                    {
                        EmployeeId = result.EmployeeId,
                        FirstName = result.FirstName,
                        LastName = result.LastName,
                        Email = result.Email,
                        PhoneNumber = result.PhoneNumber,
                        RoleId = result.RoleId,
                        DepartmentID = result.DepartmentID,
                        Images = new List<ImageTable>(),
                        Categories = new List<ProductCategory>()
                    };

                    employeeDictionary.Add(result.EmployeeId, employee);
                }

                // Add image if available
                if (result.ImageId.HasValue && !string.IsNullOrEmpty(result.ImageBase64))
                {
                    employee.Images.Add(new ImageTable
                    {
                        ImageId = result.ImageId.Value,
                        ImageBase64 = result.ImageBase64
                    });
                }

                // Add category if available
                if (result.CategoryId.HasValue)
                {
                    employee.Categories.Add(new ProductCategory
                    {
                        CategoryId = result.CategoryId.Value
                    });
                }
            }

            // Retrieve output parameter values
            int totalRecords = (int)(totalRecordsParam.Value ?? 0);
            int totalPages = (int)(totalPagesParam.Value ?? 0);

            return (employeeDictionary.Values, totalRecords, totalPages);
        }


    }

}

