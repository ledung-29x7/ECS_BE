using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class ContactRepository : IContactRepository
    {
        private readonly ECSDbContext _context;

        public ContactRepository(ECSDbContext context) 
        {
            _context = context;
        }
        public async Task AddContact(Contact contact)
        {
            var name_Param = new SqlParameter("@Name", contact.Name);
            var email_Param = new SqlParameter("@Email", contact.Email);
            var phone_Param = new SqlParameter("@PhoneNumber", contact.PhoneNumber);
            var message_Param = new SqlParameter("@Message", contact.MESSAGE);
            await _context.Database.ExecuteSqlRawAsync("EXEC AddContact @Name, @Email, @PhoneNumber, @Message", name_Param, email_Param, phone_Param, message_Param);
        }

        public async Task<List<Contact>> GetAllContact()
        {
            return await Task.FromResult(_context.contacts.FromSqlRaw("EXECUTE dbo.GetAllContact").ToList());

        }

        public async Task<Contact> GetContactById(int id)
        {
            var id_param = new SqlParameter("@ContactId", id);
            var contacts = await _context.contacts
               .FromSqlRaw("EXECUTE dbo.GetContactById @ContactId", id_param)
               .ToListAsync();
            return contacts.FirstOrDefault();
        }
    }
}
