using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CartController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CREATE A NEW CART
        [HttpPost]
        public async Task<ActionResult<Cart>> AddCart(AddCartRequest request)
        {
            // Check if product already exist in the cart, just increase the quantity and update the total
            if (_context.Carts.Any(c => c.CustomerId == request.CustomerId && c.ProductId == request.ProductId))
            {
                var cart = await _context.Carts.Where(c => c.CustomerId == request.CustomerId && c.ProductId == request.ProductId).FirstOrDefaultAsync();
                var product = await _context.Products.Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();

                cart.Quantity = cart.Quantity + 1;
                cart.Total = cart.Total + product.Price;

                await _context.SaveChangesAsync();
                return Ok("Cart is updated!");
            }
            else
            {
                var product = await _context.Products.Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();

                var cart = new Cart
                {
                    Quantity = 1,
                    Total = product.Price,
                    CustomerId = request.CustomerId,
                    ProductId = request.ProductId
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return Ok("Cart is added!");
            }
        }
    }
}
