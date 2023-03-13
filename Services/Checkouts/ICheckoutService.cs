using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.Stripe;

namespace ShopAPI.Services.Checkouts
{
    public interface ICheckoutService
    {
        Task<List<CheckoutDTO>> GetAllCheckout(int id);
        Task<CheckoutDTO> GetCheckout(int id);
        Task<StripeCustomer> AddStripeCustomer([FromBody] AddStripeCustomer customer, CancellationToken ct, int checkoutId);
        Task<StripePayment> AddStripePayment([FromBody] AddStripePayment payment, CancellationToken ct, int checkoutId);
        Task<string> AddCheckout(AddCheckoutRequest request);
    }
}
