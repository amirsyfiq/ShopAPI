namespace ShopAPI.Models.Carts
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
        public Product? Products { get; set; }
    }
}
