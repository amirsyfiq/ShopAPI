using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;
        public CustomerController(DataContext context)
        {
            _context = context;
        }

        // USER REGISTER
        [HttpPost]
        public async Task<IActionResult> Register(CustomerRegisterRequest request)
        {
            if(_context.Customers.Any(c => c.Email == request.Email))
            {
                return BadRequest("User is already exist!");
            }

            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            
            return Ok("User successfully created!");
        }

        // USER LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginRequest request)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (customer == null)
                return BadRequest("User not found!");

            if (request.Password != customer.Password)
                return BadRequest("Password is incorrect!");

            return Ok($"Welcome back {customer.Name}!");
        }
    }
}
