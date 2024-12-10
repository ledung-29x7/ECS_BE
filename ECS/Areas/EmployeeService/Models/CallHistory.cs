using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.EmployeeService.Models
{
    public class CallHistory
    {
      
        private int callId;
        private Guid employeeId;
        private DateTime callDatetime;
        private string phoneNumber;
        private int status;
        private string notes;

        [Key]
        public int CallId { get => callId; set => callId = value; }
        public Guid EmployeeId { get => employeeId; set => employeeId = value; }
        public DateTime CallDatetime { get => callDatetime; set => callDatetime = value; }
        [Required]
     
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public int Status { get => status; set => status = value; }
        public string Notes { get => notes; set => notes = value; }
    }
}
