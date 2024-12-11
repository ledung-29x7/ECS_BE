namespace ECS.Areas.Client.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public Guid ProductId { get; set; }
        public int ImageId { get; set; }
        public Product Product { get; set; }
    }
}
