namespace ECS.Areas.EmployeeService.Models
{
    public class CallStatus
    {
        private int statusId;
        private string statusName;

        public int StatusId { get => statusId; set => statusId = value; }
        public string StatusName { get => statusName; set => statusName = value; }
    }
}
