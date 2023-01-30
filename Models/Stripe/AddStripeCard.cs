namespace ShopAPI.Models.Stripe
{
    public class AddStripeCard
    {
        public string Name { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpirationYear { get; set; } = string.Empty;
        public string ExpirationMonth { get; set; } = string.Empty;
        public string Cvc { get; set; } = string.Empty;
    }
}
