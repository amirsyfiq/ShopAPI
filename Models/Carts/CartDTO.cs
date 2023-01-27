namespace ShopAPI.Models.Carts
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public ProductDTO? Products { get; set; }
    }
}
