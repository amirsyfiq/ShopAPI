﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models.Checkouts
{
    public class AddCheckoutRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public int? CustomerId { get; set; }
    }
}
