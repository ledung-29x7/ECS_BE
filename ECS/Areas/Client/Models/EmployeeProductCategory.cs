using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Client.Models
{
    public class EmployeeProductCategory
    {
        private int employeeProductCategoryId;
        private Guid employeeId;
        private int categoryId;

        [Key]
        public int EmployeeProductCategoryId { get => employeeProductCategoryId; set => employeeProductCategoryId = value; }
        public Guid EmployeeId { get => employeeId; set => employeeId = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
    }
}
