namespace ECS.Dtos
{
    public class RegisterEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? DepartmentID { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        
        public List<IFormFile> ImageFiles { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
