using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Admin.Models
{
    public class Departments
    {
        private int departmentID;
        private string departmentName;
        private Guid? managerId;

        [Key]
        public int DepartmentID { get => departmentID; set => departmentID = value; }
        public string DepartmentName { get => departmentName; set => departmentName = value; }
        public Guid? ManagerId { get => managerId; set => managerId = value; }
    }
}
