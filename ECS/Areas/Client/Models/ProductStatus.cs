using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Client.Models
{
    public class ProductStatus
    {
        private int statusId;
        private string statusName;

        [Key]
        public int StatusId { get => statusId; set => statusId = value; }
        public string StatusName { get => statusName; set => statusName = value; }
    }
}
