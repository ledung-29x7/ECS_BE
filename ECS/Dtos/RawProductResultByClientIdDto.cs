namespace ECS.Dtos
{
    public class RawProductResultByClientIdDto
    {
        public Guid ProductId { get; set; }
        public Guid ClientId { get; set; }
        public int? CategoryId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? InitialQuantity { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? TotalSold { get; set; }
        public decimal? TotalRevenue { get; set; }
        public int? StockAvailable { get; set; }
        //public string? StockStatus { get; set; }
        public string? StatusName { get; set; }
        public int? ImageId { get; set; }
        public string? ImageBase64 { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
    }
}
