using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Services.Carts
{
    public interface ICartService
    {
        Task<List<CartDTO>> GetAllItem(int id);
        Task<string> AddItem(AddCartRequest request);
        Task<string> IncreaseQuantity(int id);
        Task<string> DecreaseQuantity(int id);
        Task<string> RemoveItem(int id);
        Task<string> RemoveAllItem(int id);
    }
}
