using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var cart = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == id).Select(c => new CartDTO()
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

            var checkoutDTO = await _context.Checkouts.Include(c => c.Carts).Where(c => c.CustomerId == id).Select(c => new CheckoutDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Payment = c.Payment,
                Carts = cart
            }).ToListAsync();

            if (checkoutDTO.Count == 0)
            {
                return BadRequest("No CheckOut Found!");
            }

            return Ok(checkoutDTO);
        }


        // GET SPECIFIC CHECKOUT BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Checkout>> GetCheckout(int id) // Checkout ID
        {
            var checkout = await _context.Checkouts.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (checkout == null)
            {
                return BadRequest("No CheckOut Found!");
            }

            var cart = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == checkout.CustomerId).Select(c => new CartDTO()
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

            var checkoutDTO = await _context.Checkouts.Include(c => c.Carts).Where(c => c.CustomerId == checkout.CustomerId).Select(c => new CheckoutDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Payment = c.Payment,
                Carts = cart
            }).FirstOrDefaultAsync();

            if (checkoutDTO == null)
            {
                return BadRequest("No CheckOut Found!");
            }

            return Ok(checkoutDTO);
        }

    }
}
