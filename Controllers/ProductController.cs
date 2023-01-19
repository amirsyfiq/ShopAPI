using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models.Products;
using System.Collections.Generic;

namespace ShopAPI.Controllers
{
    [Route("ShopAPI/[controller]/[Action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET LIST OF ALL PRODUCTS
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            var product = await _context.Products.Include(p => p.Categories).ToListAsync();
            var productDTO = product.Select(p => _mapper.Map<ProductDTO>(p));

            if (productDTO == null)
                return BadRequest("There is no product at all!");

            return Ok(productDTO);
        }


        // GET SPECIFIC PRODUCT BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) // Product ID
        {
            var product = await _context.Products.Include(c => c.Categories).Where(p => p.Id == id).FirstOrDefaultAsync();
            var productDTO = _mapper.Map<ProductDTO>(product);

            if (productDTO == null)
                return BadRequest("Product is not found!");

            return Ok(productDTO);
        }


        // CREATE A NEW PRODUCTS
        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(AddProductRequest request)
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

            return Ok("Product successfully added!");
        }


        // GET LIST OF ALL CATEGORIES
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategoryList()
        {
            var category = await _context.Categories.Include(c => c.Products).ToListAsync();
            var categoryDTO = category.Select(c => _mapper.Map<CategoryDTO>(c));

            if (categoryDTO == null)
                return BadRequest("There is no category at all!");

            return Ok(categoryDTO);
        }


        // GET LIST OF PRODUCTS FOR SPECIFIC CATEGORY
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetCategory(int id) // Category ID
        {
            var product = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
            var productDTO = product.Select(p => _mapper.Map<ProductDTO>(p));

            if (productDTO?.Any() != true)
                return BadRequest("There is no product found in this categories!");

            return Ok(productDTO);
        }


        // CREATE A NEW CATEGORY
        [HttpPost]
        public async Task<ActionResult<List<Category>>> AddCategory(AddCategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok("Category successfully added!");
        }
    }
}
