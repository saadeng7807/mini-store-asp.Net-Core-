using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_store.Data;
using mini_store.Models;
using Microsoft.AspNetCore.Authorization;

namespace mini_store.Controllers
{
    // المسار المخصص للـ API في واجهة Swagger سيكون: /api/ProductsApi
    [Route("api/[controller]")]
    [ApiController] // هذا الوسم يخبر Swagger بأن هذا الكلاس مخصص بالكامل لخدمات الـ API والـ JSON
    public class ProductsApiController : ControllerBase // نرث من ControllerBase لأنه API صافي بدون Views
    {
        private readonly AppDbContext _context;

        public ProductsApiController(AppDbContext context)
        {
            _context = context;
        }





        // ----------------------------------------------------
        // 1. مسار جلب كل المنتجات أو البحث عنها: GET /api/ProductsApi
        // ----------------------------------------------------
          [HttpGet("products")]
       
        public async Task<IActionResult> GetProducts([FromQuery] string? searchTerm)
        {
            var query = _context.products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            var products = await query.ToListAsync();
            return Ok(products); // يعيد البيانات كـ JSON نقي لـ Swagger ولتطبيقات الجوال
        }

         [HttpGet("categories")]
       
        public async Task<IActionResult> GetCategories([FromQuery] string? searchTerm)
        {
            var query = await _context.categories.ToListAsync();
            return Ok(query); // يعيد البيانات كـ JSON نقي لـ Swagger ولتطبيقات الجوال
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = $"المنتج ذو الرقم {id} غير موجود" });
            }

            return Ok(product);
        }

        // ----------------------------------------------------
        // 3. مسار استقبال وإضافة منتج جديد (JSON): POST /api/ProductsApi
        // ----------------------------------------------------
      
    }
}