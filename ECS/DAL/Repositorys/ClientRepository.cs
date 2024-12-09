using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
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
        public async Task DeleteClient(Guid clientId)
        {
            var ClientId_Param = new SqlParameter("@ClientId", clientId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteClient @ClientId", ClientId_Param);
        }

        public async Task<List<Client>> GetAllClient()
        {
            return await Task.FromResult(_context.clients.FromSqlRaw("EXECUTE dbo.GetAllClient").ToList());
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
