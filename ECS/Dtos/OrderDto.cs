namespace ECS.Dtos
{
    public class OrderDto
    {
        public int CallId { get; set; }
        public string Orderer { get; set; }
        public decimal TotalAmount { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public int OrderStatus { get; set; }
    }
}
