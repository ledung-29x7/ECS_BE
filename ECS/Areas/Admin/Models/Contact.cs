using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Admin.Models
{
    public class Contact
    {
        private int contactId;
        private string name;
        private string email;
        private string phoneNumber;
        private string mESSAGE;

        [Key]
        public int ContactId { get => contactId; set => contactId = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string MESSAGE { get => mESSAGE; set => mESSAGE = value; }
    }
}
