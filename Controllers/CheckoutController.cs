using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Contracts;
using ShopAPI.Models.Products;
using ShopAPI.Models.Stripe;
using ShopAPI.Services.Checkouts;
using Stripe;
using System.Security.Claims;
using System.Web.Http.Results;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }


        // GET LIST OF ALL CHECKOUTS FOR USER
        [HttpGet, Authorize]
        public async Task<ActionResult<Checkout>> GetAllCheckout()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _checkoutService.GetAllCheckout(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }


        // GET SPECIFIC CHECKOUT BY ID
        [HttpGet, Authorize]
        public async Task<ActionResult<Checkout>> GetCheckout(int checkoutId) // Checkout ID
        {
            try
            {
                var result = await _checkoutService.GetCheckout(checkoutId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }


        // ADD DETAILS OF THE CUSTOMER/PAYERS FOR PAYMENT
        [HttpPost, Authorize]
        public async Task<ActionResult<StripeCustomer>> AddStripeCustomer([FromBody] AddStripeCustomer customer, CancellationToken ct, int checkoutId) // CheckoutId
        {
            try
            {
                var result = await _checkoutService.AddStripeCustomer(customer, ct, checkoutId);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // PROCEED TO MAKE THE PAYMENT
        [HttpPost, Authorize]
        public async Task<ActionResult<StripePayment>> AddStripePayment([FromBody] AddStripePayment payment, CancellationToken ct, int checkoutId) // CheckoutId
        {
            try
            {
                var result = await _checkoutService.AddStripePayment(payment, ct, checkoutId);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // PROCEED TO CHECKOUT ALL ITEMS IN THE CART
        [HttpPost, Authorize]
        public async Task<ActionResult<Checkout>> AddCheckout(AddCheckoutRequest request) // Checkout details with CustomerId
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _checkoutService.AddCheckout(userId, request);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
