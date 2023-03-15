using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.Products;
using System.Security.Claims;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        // GET LIST OF ALL ITEMS IN THE CART FOR USER
        [HttpGet, Authorize]
        public async Task<ActionResult<List<Cart>>> GetAllItem()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _cartService.GetAllItem(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // ADD ITEM INTO THE CART
        [HttpPost, Authorize]
        public async Task<ActionResult<Cart>> AddItem(int productId) // Product ID
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _cartService.AddItem(userId, productId);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Failed to add Item in Cart!");
            }
        }


        // INCREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut, Authorize]
        public async Task<ActionResult<Cart>> IncreaseQuantity(int cartId) // Cart ID
        {
            try
            {
                var result = await _cartService.IncreaseQuantity(cartId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // DECREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut, Authorize]
        public async Task<ActionResult<Cart>> DecreaseQuantity(int cartId) // Cart ID
        {
            try
            {
                var result = await _cartService.DecreaseQuantity(cartId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // REMOVE SPECIFIC ITEM FROM THE CART
        [HttpDelete, Authorize]
        public async Task<ActionResult<Cart>> RemoveItem(int cartId) // Cart ID
        {
            try
            {
                var result = await _cartService.RemoveItem(cartId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // REMOVE ALL ITEMS FROM THE CART
        [HttpDelete, Authorize]
        public async Task<ActionResult<Cart>> RemoveAllItem()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _cartService.RemoveAllItem(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
