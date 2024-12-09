using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Admin.Models
{
    public class Service
    {
        private int serviceId;
        private string serviceName;
        private decimal costPerDay;

        [Key]
        public int ServiceId { get => serviceId; set => serviceId = value; }
        public string ServiceName { get => serviceName; set => serviceName = value; }
        public decimal CostPerDay { get => costPerDay; set => costPerDay = value; }
    }
}
