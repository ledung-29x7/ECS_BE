using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.Units.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
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

        public async Task AddEmployeeWithImagesAsync(Employee employee, List<ImageTable> images)
        {
            var imageDataTable = new DataTable();
            imageDataTable.Columns.Add("ImageBase64", typeof(string));

            foreach (var image in images)
            {
                imageDataTable.Rows.Add(image.ImageBase64);
            }
            var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
            var LastName_param = new SqlParameter("@LastName", employee.LastName);
            var email_param = new SqlParameter("@Email", employee.Email);
            var Phone_number_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
            var Password_param = new SqlParameter("@Password", employee.Password);
            var imagesParam = new SqlParameter("@Images", SqlDbType.Structured)
            {
                TypeName = "dbo.ImageTableType",
                Value = imageDataTable
            };
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddEmployeeWithImages @FirstName, @LastName, @Email, @PhoneNumber, @Password, @Images",
                FirstName_param, LastName_param, email_param, Phone_number_param, Password_param, imagesParam
            );
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
    }
}
