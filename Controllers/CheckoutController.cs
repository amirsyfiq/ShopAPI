using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Contracts;
using ShopAPI.Models.Products;
using ShopAPI.Models.Stripe;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IStripeAppService _stripeService;

        public CheckoutController(DataContext context, IMapper mapper, IStripeAppService stripeService)
        {
            _context = context;
            _mapper = mapper;
            _stripeService = stripeService;
        }


        // GET LIST OF ALL CHECKOUTS FOR USER
        [HttpGet("{id}")]
        public async Task<ActionResult<Checkout>> GetAllCheckout(int id) // User ID
        {
            var checkoutDTO = await _context.Checkouts.Include(c => c.Carts).Where(c => c.UserId == id).Select(c => new CheckoutDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Payment = c.Payment,
                Paid= c.Paid,
                Carts = _context.Carts.Include(p => p.Products).Where(d => d.UserId == id && d.CheckoutId == c.Id).Select(d => new CartDTO()
                {
                    Id = d.Id,
                    Quantity = d.Quantity,
                    Total = d.Total,
                    Products = new ProductDTO()
                    {
                        Id = d.Products.Id,
                        Name = d.Products.Name,
                        Description = d.Products.Description,
                        Price = d.Products.Price,
                        ImageURL = d.Products.ImageURL
                    }
                }).ToList()
            }).ToListAsync();

            if (checkoutDTO.Count == 0)
                return BadRequest("No checkout found!");

            return Ok(checkoutDTO);
        }


        // GET SPECIFIC CHECKOUT BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Checkout>> GetCheckout(int id) // Checkout ID
        {
            var checkout = await _context.Checkouts.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (checkout == null)
                return BadRequest("No checkout found!");

            var cartDTO = await _context.Carts.Include(p => p.Products).Where(c => c.UserId == checkout.UserId && c.CheckoutId == id).Select(c => new CartDTO()
            {
                Id = c.Id,
                Quantity = c.Quantity,
                Total = c.Total,
                Products = new ProductDTO()
                {
                    Id = c.Products.Id,
                    Name = c.Products.Name,
                    Description = c.Products.Description,
                    Price = c.Products.Price,
                    ImageURL = c.Products.ImageURL
                }
            }).ToListAsync();

            var checkoutDTO = await _context.Checkouts.Include(c => c.Carts).Where(c => c.Id == id).Select(c => new CheckoutDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Payment = c.Payment,
                Paid= c.Paid,
                Carts = cartDTO
            }).FirstOrDefaultAsync();

            if (checkoutDTO == null)
                return BadRequest("No checkout found!");

            return Ok(checkoutDTO);
        }


        // ADD DETAILS OF THE CUSTOMER/PAYERS FOR PAYMENT
        [HttpPost]
        public async Task<ActionResult<StripeCustomer>> AddStripeCustomer([FromBody] AddStripeCustomer customer, CancellationToken ct)
        {
            StripeCustomer createdCustomer = await _stripeService.AddStripeCustomerAsync(customer, ct);
            return StatusCode(StatusCodes.Status200OK, createdCustomer);
        }


        // PROCEED TO MAKE THE PAYMENT
        [HttpPost]
        public async Task<ActionResult<StripePayment>> AddStripePayment([FromBody] AddStripePayment payment, CancellationToken ct, int checkoutId) // CheckoutId
        {
            var checkout = await _context.Checkouts.Where(c => c.Id == checkoutId).FirstOrDefaultAsync();
            long totalAmount = Convert.ToInt64(checkout.Payment * 100);

            payment.Amount = totalAmount;

            StripePayment createdPayment = await _stripeService.AddStripePaymentAsync(payment, ct);
            if (createdPayment == null)
                return BadRequest("Payment unsuccessful");

            checkout.Paid = true;
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, createdPayment);
        }


        // PROCEED TO CHECKOUT ALL ITEMS IN THE CART
        [HttpPost]
        public async Task<ActionResult<Checkout>> AddCheckout(AddCheckoutRequest request) // Checkout details with CustomerId
        {
            var cart = await _context.Carts.Where(c => c.UserId == request.UserId && c.CheckoutId == null).ToListAsync();
            if (cart.Count == 0)
                return BadRequest("No item found in the cart to checkout!");

            double totalPayment = 0;
            
            foreach(var c in cart)
            {
                totalPayment = totalPayment + c.Total;
            }

            var newcheckout = new Checkout
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = request.Address,
                Payment = totalPayment,
                Paid = false,
                UserId = request.UserId
            };

            _context.Checkouts.Add(newcheckout);
            await _context.SaveChangesAsync();
            int i = newcheckout.Id;

            foreach (var c in cart)
            {
                c.CheckoutId = i;
            }

            await _context.SaveChangesAsync();
            return Ok("Checkout completed successfully!");
        }
    }
}
