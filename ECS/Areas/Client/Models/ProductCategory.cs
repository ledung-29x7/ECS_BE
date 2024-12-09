namespace ECS.Areas.Client.Models
{
    public class ProductCategory
    {
        private int categoryId;
        private string categoryName;

        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }
    }
}
