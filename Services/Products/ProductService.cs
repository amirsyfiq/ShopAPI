using AutoMapper;
using ShopAPI.Models.Products;

namespace ShopAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // CREATE A NEW CATEGORY SERVICE
        public async Task<string> AddCategory(AddCategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return "Category successfully added!";
        }


        // CREATE A NEW PRODUCTS SERVICE
        public async Task<string> AddProduct(AddProductRequest request)
        {
            var category = await _context.Categories.Where(c => c.Id == request.categoryId).FirstOrDefaultAsync();

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageURL = request.ImageURL,
                Categories = category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return "Product successfully added!";
        }


        // GET LIST OF ALL PRODUCTS SERVICE
        public async Task<List<ProductDTO>> GetAllProduct()
        {
            var product = await _context.Products.Include(p => p.Categories).ToListAsync();
            //var productDTO = product.Select(p => _mapper.Map<ProductDTO>(p));
            var productDTO = _mapper.Map<List<ProductDTO>>(product);

            if (productDTO.Count() == 0)
                throw new ArgumentException("There is no product at all!");

            return productDTO;
        }


        // GET LIST OF PRODUCTS FOR SPECIFIC CATEGORY SERVICE
        public async Task<List<ProductDTO>> GetCategory(int id)
        {
            var product = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
            //var productDTO = product.Select(p => _mapper.Map<ProductDTO>(p));
            var productDTO = _mapper.Map<List<ProductDTO>>(product);

            if (productDTO?.Any() != true)
                throw new ArgumentException("There is no product found in this categories!");

            return productDTO;
        }


        // GET LIST OF ALL CATEGORIES SERVICE
        public async Task<List<CategoryDTO>> GetCategoryList()
        {
            var category = await _context.Categories.Include(c => c.Products).ToListAsync();
            //var categoryDTO = category.Select(c => _mapper.Map<CategoryDTO>(c));
            var categoryDTO = _mapper.Map<List<CategoryDTO>>(category);

            if (categoryDTO == null)
                throw new ArgumentException("There is no category at all!");

            return categoryDTO;
        }


        // GET SPECIFIC PRODUCT BY ID SERVICE
        public async Task<ProductDTO> GetProduct(int id)
        {
            var product = await _context.Products.Include(c => c.Categories).Where(p => p.Id == id).FirstOrDefaultAsync();
            var productDTO = _mapper.Map<ProductDTO>(product);

            if (productDTO == null)
                throw new ArgumentException("Product is not found!");

            return productDTO;
        }
    }
}
