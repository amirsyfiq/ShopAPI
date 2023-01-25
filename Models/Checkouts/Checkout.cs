namespace ShopAPI.Models.Checkouts
{
    public class Checkout
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public float Payment { get; set; }

        // RELATION WITH TABLE CUSTOMER
        public Customer? Customers { get; set; }
        public int? CustomerId { get; set; }

        // RELATION WITH TABLE CART
        public List<Cart>? Carts { get; set; }
    }
}
