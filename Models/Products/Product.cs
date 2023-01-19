namespace ShopAPI.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public string? ImageURL { get; set; } = string.Empty;

        // RELATION WITH TABLE CATEGORY
        public Category? Categories { get; set; }
        public int? CategoryId { get; set; }

        // RELATION WITH TABLE CART
        public List<Cart>? Carts { get; set; }
    }
}
