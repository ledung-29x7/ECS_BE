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
        //public List<ProductServiceRequest> ProductServices { get; set; }
    }
    public class ProductServiceRequest
    {
        public int ServiceId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RequiredEmployees { get; set; }
    }
}
