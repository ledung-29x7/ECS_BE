using ECS.Areas.Units.Models;

namespace ECS.Dtos
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentID { get; set; }
        public List<ImageTable> Images { get; set; } = new List<ImageTable>();
        public int? TotalRecords { get; set; } // Metadata for total records
        public int? TotalPages { get; set; }   // Metadata for total pages
    }
}
