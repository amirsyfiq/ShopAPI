﻿namespace ShopAPI.Models.Carts
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }

        // RELATION WITH TABLE CUSTOMER
        public Customer? Customers { get; set; }
        public int? CustomerId { get; set; }

        // RELATION WITH TABLE PRODUCT
        public Product? Products { get; set; }
        public int? ProductId { get; set; }
    }
}