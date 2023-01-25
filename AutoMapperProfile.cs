using AutoMapper;

namespace ShopAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category,CategoryDTO>();
            CreateMap<Product,ProductDTO>();
            CreateMap<Cart, CartDTO>();
            CreateMap<Checkout, CheckoutDTO>();
        }
    }
}
