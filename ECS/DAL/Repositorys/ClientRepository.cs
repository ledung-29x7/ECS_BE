using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
using ECS.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ClientRepository : IClientRepository
    {
        private readonly ECSDbContext _context;

        public ClientRepository(ECSDbContext context) 
        {
            _context = context;
        }

        public async Task ChangePassword(Guid clientId, string oldPasswordHash, string newPasswordHash)
        {
            var ClientId_Param = new SqlParameter("@ClientId", clientId);
            var clients = await _context.clients.FromSqlRaw("EXEC GetClientById @ClientId", ClientId_Param).ToListAsync();
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                throw new Exception("User not found.");
            }

            // So sánh mật khẩu người dùng nhập vào với mật khẩu đã băm trong cơ sở dữ liệu
            if (!BCrypt.Net.BCrypt.Verify(oldPasswordHash, client.Password))
            {
                throw new Exception("Old password does not match.");
            }

            // Nếu mật khẩu cũ khớp, gọi stored procedure để cập nhật mật khẩu mới
            var id_param = new SqlParameter("@ClientId", clientId);
            var newPasswordHash_param = new SqlParameter("@NewPasswordHash", newPasswordHash);

            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.ChangePasswordClient @ClientId, @NewPasswordHash", id_param, newPasswordHash_param);
        }

        public async Task DeleteClient(Guid clientId)
        {
            var ClientId_Param = new SqlParameter("@ClientId", clientId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteClient @ClientId", ClientId_Param);
        }

        public async Task<List<Client>> GetAllClient()
        {
            return await Task.FromResult(_context.clients.FromSqlRaw("EXECUTE dbo.GetAllClient").ToList());
        }

        public async Task<(IEnumerable<ClientDto> Clients, int TotalRecords, int TotalPages)> GetAllClientAndSearchAsync(int pageNumber, string searchTerm)
        {
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var searchTermParam = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);

            var result = await _context.clientDtos
                .FromSqlInterpolated($"EXEC GetAllClientAndSearch @PageNumber = {pageNumberParam}, @SearchTerm = {searchTermParam}")
                .ToListAsync();

            // Extract metadata from the first item (assuming all items have the same TotalRecords and TotalPages).
            int totalRecords = result.FirstOrDefault()?.TotalRecords ?? 0;
            int totalPages = result.FirstOrDefault()?.TotalPages ?? 0;

            return (result, totalRecords, totalPages);
        }

        public async Task<Client> GetClientByEmail(string email)
        {
            var email_Param = new SqlParameter("@Email", email);
            var clients = await _context.clients.FromSqlRaw("EXEC GetClientByEmail @Email", email_Param).ToListAsync();
            return clients.FirstOrDefault();
        }

        public async Task<Client> GetClientById(Guid clientId)
        {
            var ClientId_Param = new SqlParameter("@ClientId", clientId);
            var clients = await _context.clients.FromSqlRaw("EXEC GetClientById @ClientId", ClientId_Param).ToListAsync();
            return clients.FirstOrDefault();
        }

        public async Task RegisterClient(Client client)
        {
            var ClientName_Param = new SqlParameter("@ClientName", client.ClientName);
            var ContactPerson_Param = new SqlParameter("@ContactPerson", client.ContactPerson);
            var Email_Param = new SqlParameter("@Email", client.Email);
            var PhoneNumber_Param = new SqlParameter("@PhoneNumber", client.PhoneNumber);
            var Address_Param = new SqlParameter("@Address", client.Address);
            var Password_Param = new SqlParameter("@Password", client.Password);
            await _context.Database.ExecuteSqlRawAsync("EXEC RegisterClient @ClientName, @ContactPerson, @Email, @PhoneNumber, @Address, @Password", ClientName_Param, ContactPerson_Param, Email_Param, PhoneNumber_Param, Address_Param, Password_Param);
        }

        public async Task UpdateClient(Client client)
        {
            var ClientId_Param = new SqlParameter("@ClientId", client.ClientId);
            var ClientName_Param = new SqlParameter("@ClientName", client.ClientName);
            var ContactPerson_Param = new SqlParameter("@ContactPerson", client.ContactPerson);
            var Email_Param = new SqlParameter("@Email", client.Email);
            var PhoneNumber_Param = new SqlParameter("@PhoneNumber", client.PhoneNumber);
            var Address_Param = new SqlParameter("@Address", client.Address);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateClient @ClientId, @ClientName, @ContactPerson, @Email, @PhoneNumber, @Address", ClientId_Param, ClientName_Param, ContactPerson_Param, Email_Param, PhoneNumber_Param, Address_Param);
        }
    }
}
