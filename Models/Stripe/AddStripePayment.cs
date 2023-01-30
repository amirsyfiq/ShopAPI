namespace ShopAPI.Models.Stripe
{
    public class AddStripePayment
    {
        public string CustomerId { get; set; } = string.Empty;
        public string ReceiptEmail { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        [JsonIgnore]
        public long Amount;
    }
}
