using ECS.Areas.Admin.Models;

namespace ECS.DAL.Interfaces
{
    public interface IContactRepository
    {
        Task AddContact(Contact contact);
        Task<List<Contact>> GetAllContact();

        Task<Contact> GetContactById(int id);
    }
}
