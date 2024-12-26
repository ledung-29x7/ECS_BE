namespace ECS.Dtos
{
    public class ClientDto
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int TotalRecords { get; set; } // Metadata for total records
        public int TotalPages { get; set; }   // Metadata for total pages
    }
}
