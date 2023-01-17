using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Product
{
    public class AddCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
