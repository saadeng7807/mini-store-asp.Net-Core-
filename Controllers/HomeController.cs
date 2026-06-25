using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mini_store.Models;
using mini_store.Data;
namespace mini_store.Controllers;

public class HomeController : Controller
{
   private readonly AppDbContext _context;


   public HomeController(AppDbContext cn)
    {
        _context=cn;
    }
 private static  dynamic[] _products  =
{
    new { CategoryId = 0, Name = "هاتف ذكي", Price = 2500, Description = "هاتف ذكي بكاميرا عالية الدقة", Image = "phone.jpg" },

    new { CategoryId = 0, Name = "حاسوب محمول", Price = 4500, Description = "حاسوب مخصص للمطورين ram 16gm hard 1TB screen 16hs", Image = "laptop.jpg" },

    new { CategoryId = 1, Name = "قميص قطني", Price = 150, Description = "قميص مريح وصيفي", Image = "shirt.jpg" },

    new { CategoryId = 2, Name = "كتاب برمجة", Price = 90, Description = "دليل شامل لتعلم البرمجة", Image = "book.jpg" }
};


    public IActionResult Index()
    {
       var Categories=_context.categories.ToList();

        return View(Categories);
    }


    [Route("list")]
    public IActionResult Products(int id)
    {
        var filtered=_products.Where(p=> p.CategoryId==id).ToList();
         ViewBag.Filtered=filtered;
        return View();
    }

    public IActionResult Details(string name)
    {

         var filtered=_products.FirstOrDefault(p=> p.Name== name);

         ViewBag.Products=filtered;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
