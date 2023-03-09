using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Services.Users
{
    public interface IUserService
    {
        Task<string> Register(UserRegisterRequest request);
        Task<string> Login(UserLoginRequest request);
        Task<string> GetUser();
    }
}
