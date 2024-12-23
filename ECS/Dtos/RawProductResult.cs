namespace ECS.Dtos
{
    public class RawProductResult
    {
        public Guid ProductId { get; set; }
        public Guid ClientId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int InitialQuantity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Image Information
        public int? ImageId { get; set; }
        public string? ImageBase64 { get; set; }

        // Metadata
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
