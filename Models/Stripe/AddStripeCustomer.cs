namespace ShopAPI.Models.Stripe
{
    public class AddStripeCustomer
    {
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public string Email { get; set; } = string.Empty;
        public AddStripeCard CreditCard { get; set; }
    }
}
