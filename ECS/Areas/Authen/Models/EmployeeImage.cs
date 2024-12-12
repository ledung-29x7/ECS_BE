using ECS.Areas.Client.Models;

namespace ECS.Areas.Authen.Models
{
    public class EmployeeImage
    {
        public int EmployeeImageId { get; set; }
        public Guid EmployeeId { get; set; }
        public int ImageId { get; set; }
        public Employee Employee { get; set; }
    }
}
