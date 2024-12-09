using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Units.Models
{
    public class ImageTable
    {
        private int imageId;
        private string imageBase64;

        [Key]
        public int ImageId { get => imageId; set => imageId = value; }
        public string ImageBase64 { get => imageBase64; set => imageBase64 = value; }
    }
}
