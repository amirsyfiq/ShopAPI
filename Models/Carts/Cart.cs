namespace ShopAPI.Models.Carts
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }

        // RELATION WITH TABLE CUSTOMER
        public User? Users { get; set; }
        public int? UserId { get; set; }

        // RELATION WITH TABLE PRODUCT
        public Product? Products { get; set; }
        public int? ProductId { get; set; }

        // RELATION WITH TABLE CHECKOUT
        public Checkout? Checkouts { get; set; }
        public int? CheckoutId { get; set; }
    }
}
