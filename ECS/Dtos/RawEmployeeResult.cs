namespace ECS.Dtos
{
    public class RawEmployeeResult
    {
        public Guid EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentID { get; set; }
        public int? ImageId { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
