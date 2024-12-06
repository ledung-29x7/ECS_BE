namespace ECS.Areas.Admin.Models
{
    public class Client
    {
     private Guid clientId;
     private string clientName;
     private string contactPerson;
     private string email;
     private string phoneNumber;
     private string address;
     private string password;
     private DateTime? createAt;

        public Guid ClientId { get => clientId; set => clientId = value; }
        public string ClientName { get => clientName; set => clientName = value; }
        public string ContactPerson { get => contactPerson; set => contactPerson = value; }
        public string Email { get => email; set => email = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Address { get => address; set => address = value; }
        public string Password { get => password; set => password = value; }
        public DateTime? CreateAt { get => createAt; set => createAt = value; }
    }
}
