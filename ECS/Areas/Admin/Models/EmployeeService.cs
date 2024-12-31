namespace ECS.Areas.Admin.Models
{
    public class EmployeeService
    {
        private int employeeServiceId;
        private Guid employeeId;
        private int productServiceId;
        private DateTime? assignedAt;

        public int EmployeeServiceId { get => employeeServiceId; set => employeeServiceId = value; }
        public Guid EmployeeId { get => employeeId; set => employeeId = value; }
        public int ProductServiceId { get => productServiceId; set => productServiceId = value; }
        public DateTime? AssignedAt { get => assignedAt; set => assignedAt = value; }
    }
}
