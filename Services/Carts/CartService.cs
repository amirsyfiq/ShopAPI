using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CartService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET LIST OF ALL ITEMS IN THE CART FOR USER SERVICE
        public async Task<List<CartDTO>> GetAllItem(int id) // User ID
        {
            var cartDTO = await _context.Carts.Include(p => p.Products).Where(c => c.UserId == id && c.CheckoutId == null).Select(c => new CartDTO()
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
                throw new ArgumentException("Cart is empty!");

            return cartDTO;
        }


        // ADD ITEM INTO THE CART SERVICE
        public async Task<string> AddItem(AddCartRequest request) // User ID and Product ID as object
        {
            // Check if product already exist in the cart, just increase the quantity and update the total
            if (_context.Carts.Any(c => c.UserId == request.UserId && c.ProductId == request.ProductId))
            {
                var cart = await _context.Carts.Where(c => c.UserId == request.UserId && c.ProductId == request.ProductId && c.CheckoutId == null).FirstOrDefaultAsync();
                var product = await _context.Products.Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();

                if (cart == null)
                {
                    var newcart = new Cart
                    {
                        Quantity = 1,
                        Total = product.Price,
                        UserId = request.UserId,
                        Products = product
                    };

                    _context.Carts.Add(newcart);
                    await _context.SaveChangesAsync();
                    return "Cart is added!";
                }
                else
                {
                    cart.Quantity = cart.Quantity + 1;
                    cart.Total = cart.Total + product.Price;

                    await _context.SaveChangesAsync();
                    return "Cart is updated!";
                }
            }
            else
            {
                var product = await _context.Products.Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();

                var newcart = new Cart
                {
                    Quantity = 1,
                    Total = product.Price,
                    UserId = request.UserId,
                    Products = product
                };

                _context.Carts.Add(newcart);
                await _context.SaveChangesAsync();
                return "Cart is added!";
            }
        }


        // INCREASE QUANTITY OF SPECIFIC ITEM IN THE CART SERVICE
        public async Task<string> IncreaseQuantity(int id) // Cart ID
        {
            var cart = await _context.Carts.Where(c => c.Id == id && c.CheckoutId == null).FirstOrDefaultAsync();
            if (cart == null)
                throw new ArgumentException("Item not found!");

            var product = await _context.Products.Where(p => p.Id == cart.ProductId).FirstOrDefaultAsync();
            if (product == null)
                throw new ArgumentException("Item not found!");

            cart.Quantity = cart.Quantity + 1;
            cart.Total = cart.Total + product.Price;

            await _context.SaveChangesAsync();
            return "Item quantity is updated!";
        }


        // DECREASE QUANTITY OF SPECIFIC ITEM IN THE CART SERVICE
        public async Task<string> DecreaseQuantity(int id) // Cart ID
        {
            var cart = await _context.Carts.Where(c => c.Id == id && c.CheckoutId == null).FirstOrDefaultAsync();
            if (cart == null)
                throw new ArgumentException("Item not found!");

            var product = await _context.Products.Where(p => p.Id == cart.ProductId).FirstOrDefaultAsync();
            if (product == null)
                throw new ArgumentException("Item not found!");

            // Check if remaining product quantity in the cart is only one, remove the product from the cart
            if (cart.Quantity <= 1)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return "Item has been removed!";
            }
            else
            {
                cart.Quantity = cart.Quantity - 1;
                cart.Total = cart.Total - product.Price;

                await _context.SaveChangesAsync();
                return "Item quantity is updated!";
            }
        }


        // REMOVE SPECIFIC ITEM FROM THE CART SERVICE
        public async Task<string> RemoveItem(int id) // Cart ID
        {
            var cart = await _context.Carts.Where(c => c.Id == id && c.CheckoutId == null).FirstOrDefaultAsync();
            if (cart == null)
                throw new ArgumentException("Item not found!");

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return "Item has been removed!";
        }


        // REMOVE ALL ITEMS FROM THE CART SERVICE
        public async Task<string> RemoveAllItem(int id) // User ID
        {
            var cart = await _context.Carts.Include(p => p.Products).Where(c => c.UserId == id && c.CheckoutId == null).ToListAsync();
            if (cart?.Any() != true)
               throw new ArgumentException("Cart is empty!");

            _context.Carts.RemoveRange(cart);
            await _context.SaveChangesAsync();

            return "All items has been removed!";
        }

    }
}
