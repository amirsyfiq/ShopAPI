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
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        // GET LIST OF ALL PRODUCTS
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            try
            {
                var result = await _productService.GetAllProduct();
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET SPECIFIC PRODUCT BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) // Product ID
        {
            try
            {
                var result = await _productService.GetProduct(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // CREATE A NEW PRODUCTS
        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(AddProductRequest request)
        {
            try
            {
                var result = await _productService.AddProduct(request);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Failed to add New Product!");
            }
        }


        // GET LIST OF ALL CATEGORIES
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategoryList()
        {
            try
            {
                var result = await _productService.GetCategoryList();
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET LIST OF PRODUCTS FOR SPECIFIC CATEGORY
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetCategory(int id) // Category ID
        {
            try
            {
                var result = await _productService.GetCategory(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        // CREATE A NEW CATEGORY
        [HttpPost]
        public async Task<ActionResult<string>> AddCategory(AddCategoryRequest request)
        {
            try
            {
                var result = await _productService.AddCategory(request);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Failed to add New Category!");
            }
        }
    }
}
