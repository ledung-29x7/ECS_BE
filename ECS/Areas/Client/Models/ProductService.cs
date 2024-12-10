namespace ECS.Areas.Client.Models
{
    public class ProductService
    {
        private int productServiceId;
        private int serviceId;
        private Guid productId;
        private Guid clientId;
        private DateTime startDate;
        private DateTime endDate;
        private int requiredEmployees;

        public int ProductServiceId { get => productServiceId; set => productServiceId = value; }
        public int ServiceId { get => serviceId; set => serviceId = value; }
        public Guid ProductId { get => productId; set => productId = value; }
        public Guid ClientId { get => clientId; set => clientId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public int RequiredEmployees { get => requiredEmployees; set => requiredEmployees = value; }
    }
}
