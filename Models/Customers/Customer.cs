namespace ShopAPI.Models.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // RELATION WITH TABLE CART
        public List<Cart>? Carts { get; set; }
    }
}
