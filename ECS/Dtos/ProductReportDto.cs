namespace ECS.Dtos
{
    public class ProductReportDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int StockAvailable { get; set; }
    }
}
