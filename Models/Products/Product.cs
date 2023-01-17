namespace ShopAPI.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }

        // RELATION WITH TABLE CATEGORY
        public Category? Categories { get; set; }
        public int? CategoryId { get; set; }

        // RELATION WITH TABLE CART
        public List<Cart>? Carts { get; set; }

        // RELATION WITH TABLE PRODUCT IMAGE
        //public ProductImage ProductImages { get; set; }
    }

    //public class ProductImage
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; } = string.Empty;
    //    public IFormFile? Image { get; set; }

    //    // RELATION WITH TABLE CATEGORY
    //    public Product? Products { get; set; }
    //    public int? ProductId { get; set; }
    //}
}
