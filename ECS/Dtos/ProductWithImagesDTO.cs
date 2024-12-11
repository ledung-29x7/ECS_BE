using ECS.Areas.Units.Models;

namespace ECS.Dtos
{
    public class ProductWithImagesDTO
    {
        public Guid ProductId { get; set; }
        public Guid ClientId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int InitialQuantity { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ImageTable> Images { get; set; } = new List<ImageTable>();
    }
}
