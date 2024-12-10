namespace ECS.Areas.EmployeeService.Models
{
    public class OrderDetail
    {
        private int orderDetailId;
        private int orderId;
        private Guid productId;
        private int quantity;
        private decimal totalPrice;

        public int OrderDetailId { get => orderDetailId; set => orderDetailId = value; }
        public int OrderId { get => orderId; set => orderId = value; }
        public Guid ProductId { get => productId; set => productId = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public decimal TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
