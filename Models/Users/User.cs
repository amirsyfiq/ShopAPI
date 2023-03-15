using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


        // RELATION WITH TABLE CART
        public List<Cart>? Carts { get; set; }

        // RELATION WITH TABLE CHECKOUT
        public List<Checkout>? Checkouts { get; set; }
    }
}
