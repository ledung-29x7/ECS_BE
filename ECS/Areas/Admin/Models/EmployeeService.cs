namespace ECS.Areas.Admin.Models
{
    public class EmployeeService
    {
        private int employeeServiceId;
        private Guid employeeId;
        private int serviceId;

        public int EmployeeServiceId { get => employeeServiceId; set => employeeServiceId = value; }
        public Guid EmployeeId { get => employeeId; set => employeeId = value; }
        public int ServiceId { get => serviceId; set => serviceId = value; }
    }
}
