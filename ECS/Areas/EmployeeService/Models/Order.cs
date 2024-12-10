using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.EmployeeService.Models
{
    public class Order
    {
        [Key]
        private int orderId;
        private int callId;
        private string orderer;
        private DateTime orderDate;
        private decimal totalAmount;
        private string recipient_Name;
        private string recipient_Phone;
        private string recipient_Address;
        private int orderStatus;

        public int OrderId { get => orderId; set => orderId = value; }
        public int CallId { get => callId; set => callId = value; }
        public string Orderer { get => orderer; set => orderer = value; }
        public decimal TotalAmount { get => totalAmount; set => totalAmount = value; }
        public string Recipient_Name { get => recipient_Name; set => recipient_Name = value; }
        public string Recipient_Phone { get => recipient_Phone; set => recipient_Phone = value; }
        public string Recipient_Address { get => recipient_Address; set => recipient_Address = value; }
        public int OrderStatus { get => orderStatus; set => orderStatus = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
    }
}
