namespace ECS.Areas.Client.Models
{
    public class Product
    {
        private Guid productId;
        private Guid clientId;
        private int? categoryId;
        private string productName; 
        private decimal? price;
        private int? initialQuantity;
        private string description;
        private Boolean isActive;
        private int? status;
        private DateTime? createdAt;

        public Guid ProductId { get => productId; set => productId = value; }
        public Guid ClientId { get => clientId; set => clientId = value; }
        public int? CategoryId { get => categoryId; set => categoryId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public decimal? Price { get => price; set => price = value; }
        public int? InitialQuantity { get => initialQuantity; set => initialQuantity = value; }
        public string Description { get => description; set => description = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public int? Status { get => status; set => status = value; }
        public DateTime? CreatedAt { get => createdAt; set => createdAt = value; }
    }
}
