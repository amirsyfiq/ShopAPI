using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }


        // USER REGISTER
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_context.Users.Any(c => c.Email == request.Email))
                return BadRequest("User is already exist!");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return Ok("User successfully created!");
        }


        // USER LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (user == null)
                return BadRequest("User not found!");

            if (request.Password != user.Password)
                return BadRequest("Password is incorrect!");

            return Ok($"Welcome back {user.Name}!");
        }
    }
}
