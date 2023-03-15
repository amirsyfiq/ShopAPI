using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.Users;

namespace ShopAPI.Services.Carts
{
    public interface ICartService
    {
        Task<List<CartDTO>> GetAllItem(int userId);
        Task<string> AddItem(int userId, int productId);
        Task<string> IncreaseQuantity(int cartId);
        Task<string> DecreaseQuantity(int cartId);
        Task<string> RemoveItem(int cartId);
        Task<string> RemoveAllItem(int userId);
    }
}
