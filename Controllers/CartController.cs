using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.Products;

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

        // GET LIST OF ITEMS IN THE CART FOR USER
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetAllItem(int id)
        {
            var cart = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == id).ToListAsync();
            var cartDTO = cart.Select(c => _mapper.Map<CartDTO>(c));

            if (cartDTO?.Any() != true)
                return BadRequest("Cart is empty!");

            return Ok(cartDTO);
        }

        // ADD ITEM INTO THE CART
        [HttpPost]
        public async Task<ActionResult<Cart>> AddItem(AddCartRequest request)
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
                    Products = product
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return Ok("Cart is added!");
            }
        }

        // INCREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut]
        public async Task<ActionResult<List<Cart>>> IncreaseQuantity(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            var product = await _context.Products.Where(p => p.Id == cart.ProductId).FirstOrDefaultAsync();
            
            if (cart == null || product == null)
                return BadRequest("Item not found!");

            cart.Quantity = cart.Quantity + 1;
            cart.Total = cart.Total + product.Price;

            await _context.SaveChangesAsync();
            return Ok("Item quantity is updated!");
        }

        // DECREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut]
        public async Task<ActionResult<List<Cart>>> DecreaseQuantity(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            var product = await _context.Products.Where(p => p.Id == cart.ProductId).FirstOrDefaultAsync();

            if (cart == null || product == null)
                return BadRequest("Item not found!");

            // Check if remaining product quantity in the cart is only one, remove the product from the cart
            if (cart.Quantity <= 1)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return Ok("Item has been removed!");
            }
            else
            {
                cart.Quantity = cart.Quantity - 1;
                cart.Total = cart.Total - product.Price;

                await _context.SaveChangesAsync();
                return Ok("Item quantity is updated!");
            }
        }

        // REMOVE SPECIFIC ITEM FROM THE CART
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cart>>> RemoveItem(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return BadRequest("Item not found!");

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok("Item has been removed!");
        }

        // REMOVE ALL ITEMS FROM THE CART
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cart>>> RemoveAllItem(int id)
        {
            var cart = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == id).ToListAsync();
            //var cart = await _context.Carts.Where(c => c.CustomerId == id).ToListAsync();
            if (cart?.Any() != true)
                return BadRequest("Cart is empty!");

            _context.Carts.RemoveRange(cart);
            await _context.SaveChangesAsync();

            return Ok("All items has been removed!");
        }
    }
}
