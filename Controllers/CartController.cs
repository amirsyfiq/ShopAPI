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
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        // GET LIST OF ALL ITEMS IN THE CART FOR USER
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Cart>>> GetAllItem(int id) // User ID
        {
            try
            {
                var result = await _cartService.GetAllItem(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // ADD ITEM INTO THE CART
        [HttpPost]
        public async Task<ActionResult<Cart>> AddItem(AddCartRequest request) // User ID and Product ID as object
        {
            try
            {
                var result = await _cartService.AddItem(request);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Failed to add Item in Cart!");
            }
        }


        // INCREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut]
        public async Task<ActionResult<Cart>> IncreaseQuantity(int id) // Cart ID
        {
            try
            {
                var result = await _cartService.IncreaseQuantity(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // DECREASE QUANTITY OF SPECIFIC ITEM IN THE CART
        [HttpPut]
        public async Task<ActionResult<Cart>> DecreaseQuantity(int id) // Cart ID
        {
            try
            {
                var result = await _cartService.DecreaseQuantity(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // REMOVE SPECIFIC ITEM FROM THE CART
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> RemoveItem(int id) // Cart ID
        {
            try
            {
                var result = await _cartService.RemoveItem(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // REMOVE ALL ITEMS FROM THE CART
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> RemoveAllItem(int id) // User ID
        {
            try
            {
                var result = await _cartService.RemoveAllItem(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
