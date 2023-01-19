using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Products
{
    public class AddProductRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public float Price { get; set; }
        [Required]
        public string ImageURL { get; set; } = string.Empty;
        [Required]
        public int categoryId { get; set; }
    }
}
