﻿namespace ShopAPI.Models.Checkouts
{
    public class CheckoutDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public float Payment { get; set; }
        public List<CartDTO>? Carts { get; set; }
    }
}