using Microsoft.AspNetCore.Mvc;
using mini_store.Data;
using mini_store.Models;

namespace mini_store.Controllers
{
    public class ProductsDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsDetailsController(AppDbContext context)
        {
            
            _context = context;
        }

        public IActionResult Index()
        {
            var productDetails = _context.productDetails.ToList();
            return View(productDetails);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var products = _context.products.ToList();
            ViewBag.Products = products;
            return View();
        }

         

        [HttpPost]
        public IActionResult Create(ProductDetails productDetails)
        {
            if (ModelState.IsValid)
            {
                _context.productDetails.Add(productDetails);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var products = _context.products.ToList();
            ViewBag.Products = products;
            return View(productDetails);
        }

        public IActionResult Details(int id)
        {
            var productDetails = _context.productDetails.FirstOrDefault(pd => pd.Id == id);
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }
    }



    
}