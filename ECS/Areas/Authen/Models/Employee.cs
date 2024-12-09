using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Authen.Models
{
    public class Employee
    {
        private Guid employeeId;
        private string? firstName;
        private string? lastName;
        private string? email;
        private string? phoneNumber;
        private int roleId;
        private int? departmentID;
        private string password;
        private DateTime? createdAt;

        [Key]
        public Guid EmployeeId { get => employeeId; set => employeeId = value; }
        public string? FirstName { get => firstName; set => firstName = value; }
        public string? LastName { get => lastName; set => lastName = value; }
        public string? Email { get => email; set => email = value; }
        public string? PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public int RoleId { get => roleId; set => roleId = value; }
        public string Password { get => password; set => password = value; }
        public int? DepartmentID { get => departmentID; set => departmentID = value; }
        public DateTime? CreatedAt { get => createdAt; set => createdAt = value; }
    }
}
