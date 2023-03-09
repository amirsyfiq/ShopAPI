using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // USER REGISTER
        [HttpPost]
        public async Task<ActionResult<User>> Register(UserRegisterRequest request)
        {
            try
            {
                var result = await _userService.Register(request);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // USER LOGIN
        [HttpPost]
        public async Task<ActionResult<User>> Login(UserLoginRequest request)
        {
            try
            {
                var result = await _userService.Login(request);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET DETAILS OF CURRENT USER (USER ID)
        [HttpGet, Authorize]
        public ActionResult<string> GetUser()
        {
            var result = User.FindFirstValue("UserId");
            //var result = _userService.GetUser();
            return Ok(result);
        }
    }
}
