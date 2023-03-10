using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProduct();
        Task<ProductDTO> GetProduct(int id);
        Task<string> AddProduct(AddProductRequest request);
        Task<List<CategoryDTO>> GetCategoryList();
        Task<List<ProductDTO>> GetCategory(int id);
        Task<string> AddCategory(AddCategoryRequest request);

    }
}
