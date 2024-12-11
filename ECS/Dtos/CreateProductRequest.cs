namespace ECS.Dtos
{
    public class CreateProductRequest
    {
        public Guid ClientId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int InitialQuantity { get; set; }
        public string Description { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }
}
