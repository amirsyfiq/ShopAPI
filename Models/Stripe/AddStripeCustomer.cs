namespace ShopAPI.Models.Stripe
{
    public class AddStripeCustomer
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddStripeCard? CreditCard { get; set; }
    }
}
