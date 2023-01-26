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


        // GET LIST OF ALL ITEMS IN THE CART FOR USER
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetAllItem(int id) // User ID
        {
            var cartDTO = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == id && c.CheckoutId == null).Select(c => new CartDTO()
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

            if (cartDTO.Count == 0)
                return BadRequest("Cart is empty!");

            return Ok(cartDTO);
        }


        // ADD ITEM INTO THE CART
        [HttpPost]
        public async Task<ActionResult<Cart>> AddItem(AddCartRequest request) // User ID and Product ID as object
        {
            // Check if product already exist in the cart, just increase the quantity and update the total
            if (_context.Carts.Any(c => c.CustomerId == request.CustomerId && c.ProductId == request.ProductId))
            {
                var cart = await _context.Carts.Where(c => c.CustomerId == request.CustomerId && c.ProductId == request.ProductId && c.CheckoutId == null).FirstOrDefaultAsync();
                var product = await _context.Products.Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();

                if(cart == null)
                {
                     var newcart = new Cart
                    {
                        Quantity = 1,
                        Total = product.Price,
                        CustomerId = request.CustomerId,
                        Products = product
                    };

                    _context.Carts.Add(newcart);
                    await _context.SaveChangesAsync();
                    return Ok("Cart is added!");
                }
                else
                {
                    cart.Quantity = cart.Quantity + 1;
                    cart.Total = cart.Total + product.Price;

                    await _context.SaveChangesAsync();
                    return Ok("Cart is updated!");
                }
            }
            else
            {
                var product = await _context.Products.Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();

                var newcart = new Cart
                {
                    Quantity = 1,
                    Total = product.Price,
                    CustomerId = request.CustomerId,
                    Products = product
                };

                _context.Carts.Add(newcart);
                await _context.SaveChangesAsync();
                return Ok("Cart is added!");
            }
        }


        // INCREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut]
        public async Task<ActionResult<List<Cart>>> IncreaseQuantity(int id) // Cart ID
        {
            var cart = await _context.Carts.Where(c => c.Id == id && c.CheckoutId == null).FirstOrDefaultAsync();
            if (cart == null)
                return BadRequest("Item not found!");

            var product = await _context.Products.Where(p => p.Id == cart.ProductId).FirstOrDefaultAsync();
            if (product == null)
                return BadRequest("Item not found!");

            cart.Quantity = cart.Quantity + 1;
            cart.Total = cart.Total + product.Price;

            await _context.SaveChangesAsync();
            return Ok("Item quantity is updated!");
        }


        // DECREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut]
        public async Task<ActionResult<List<Cart>>> DecreaseQuantity(int id) // Cart ID
        {
            var cart = await _context.Carts.Where(c => c.Id == id && c.CheckoutId == null).FirstOrDefaultAsync();
            if (cart == null)
                return BadRequest("Item not found!");

            var product = await _context.Products.Where(p => p.Id == cart.ProductId).FirstOrDefaultAsync();
            if (product == null)
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
        public async Task<ActionResult<List<Cart>>> RemoveItem(int id) // Cart ID
        {
            var cart = await _context.Carts.Where(c => c.Id == id && c.CheckoutId == null).FirstOrDefaultAsync();
            if (cart == null)
                return BadRequest("Item not found!");

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok("Item has been removed!");
        }


        // REMOVE ALL ITEMS FROM THE CART
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cart>>> RemoveAllItem(int id) // User ID
        {
            var cart = await _context.Carts.Include(p => p.Products).Where(c => c.CustomerId == id && c.CheckoutId == null).ToListAsync();
            if (cart?.Any() != true)
                return BadRequest("Cart is empty!");

            _context.Carts.RemoveRange(cart);
            await _context.SaveChangesAsync();

            return Ok("All items has been removed!");
        }
    }
}
