using Microsoft.AspNetCore.Mvc;
using mini_store.Models;
using mini_store.Data;
using Microsoft.AspNetCore.Authorization;
namespace mini_store.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
       public static int  Amount=100;

        public CategoriesController(AppDbContext cn )
        {
            _context=cn;
        }
         [Authorize]
        public IActionResult Index()
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7); // تعيين مدة صلاحية
            
            HttpContext.Session.SetString("UsrName", "أحمد"); // تخزين القيمة في الجلسة

            Response.Cookies.Append("Costumers", "Ahmed",options);
            Response.Cookies.Append("Course", "Asp.net Core MVC",options);
              Response.Cookies.Append("Order", "250",options);
            
            var categories=_context.categories.ToList();
            return View(categories);
        }
        

        [HttpPost]
        public IActionResult Create(Categories categories)
        {
          

            return RedirectToAction("Index");

        }
    }
}