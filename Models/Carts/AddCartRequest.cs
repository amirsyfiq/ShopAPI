using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Carts
{
    public class AddCartRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
