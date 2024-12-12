using ECS.Areas.Units.Models;

namespace ECS.Dtos
{
    public class EmployeeWithImagesDTO
    {
        public Guid EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public List<ImageTable> Images { get; set; } = new List<ImageTable>();
    }
}
