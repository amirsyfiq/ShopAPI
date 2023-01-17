namespace ShopAPI.Models.Products
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // RELATION WITH TABLE PRODUCT
        public List<Product> Products { get; set; }
    }
}
