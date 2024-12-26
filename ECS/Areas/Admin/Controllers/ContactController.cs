using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // POST: api/Contact
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest("Contact is null.");
            }

            await _contactRepository.AddContact(contact);
            return Ok("Contact added successfully.");
        }

        // GET: api/Contact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {
            var contacts = await _contactRepository.GetAllContact();
            return Ok(contacts);
        }

        // GET: api/Contact/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContactById(int id)
        {
            var contact = await _contactRepository.GetContactById(id);

            if (contact == null)
            {
                return NotFound($"Contact with Id = {id} not found.");
            }

            return Ok(contact);
        }
    }
}
