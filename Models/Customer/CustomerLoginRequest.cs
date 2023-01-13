using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Customer
{
    public class CustomerLoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
