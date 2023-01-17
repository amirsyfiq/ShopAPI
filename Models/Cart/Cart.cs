namespace ShopAPI.Models.Cart
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        // RELATION WITH TABLE CUSTOMER
        //public Customer? Customers { get; set; }
        //public int? CustomerId { get; set; }
    }
}
