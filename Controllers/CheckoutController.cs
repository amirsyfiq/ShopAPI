using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.Products;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CheckoutController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET LIST OF ALL CHECKOUTS FOR USER
        [HttpGet("{id}")]
        public async Task<ActionResult<Checkout>> GetAllCheckout(int id) // User ID
        {
            var checkoutDTO = await _context.Checkouts.Include(c => c.Carts).Where(c => c.CustomerId == id).Select(c => new CheckoutDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Payment = c.Payment,
                Carts = _context.Carts.Include(p => p.Products).Where(d => d.CustomerId == id && d.CheckoutId == c.Id).Select(d => new CartDTO()
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

            var cartDTO = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == checkout.CustomerId && c.CheckoutId == id).Select(c => new CartDTO()
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
                Carts = cartDTO
            }).FirstOrDefaultAsync();

            if (checkoutDTO == null)
                return BadRequest("No checkout found!");

            return Ok(checkoutDTO);
        }


        // PROCEED TO CHECKOUT ALL ITEMS IN THE CART
        [HttpPost]
        public async Task<ActionResult<Checkout>> AddCheckout(AddCheckoutRequest request) // Checkout details with CustomerId
        {
            var cart = await _context.Carts.Where(c => c.CustomerId == request.CustomerId && c.CheckoutId == null).ToListAsync();
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
                CustomerId= request.CustomerId
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
