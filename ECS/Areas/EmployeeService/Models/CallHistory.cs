using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;

namespace ECS.Areas.EmployeeService.Models
{
    public class CallHistory
    {
        public int callId;
        public Guid employeeId;
        public DateTime callDatetime;
        public string phoneNumber;
        public int status;
        public string notes;

        [Key]

        public int CallId { get => callId; set => callId = value; }
        public Guid Employee { get => employeeId; set => employeeId = value; }
        public DateTime CallDatetime { get => callDatetime; set => callDatetime = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public int Status { get => status; set => status = value; }
        public string Notes { get => notes; set => notes = value; }
    }
}