namespace ShopAPI.Models.Checkouts
{
    public class Checkout
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Payment { get; set; }
        public bool Paid { get; set; }

        // RELATION WITH TABLE CUSTOMER
        public User? Users { get; set; }
        public int? UserId { get; set; }

        // RELATION WITH TABLE CART
        public List<Cart>? Carts { get; set; }
    }
}
