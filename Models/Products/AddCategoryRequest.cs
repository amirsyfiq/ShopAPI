using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Products
{
    public class AddCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
