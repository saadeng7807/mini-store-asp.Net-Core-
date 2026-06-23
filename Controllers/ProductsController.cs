using Microsoft.AspNetCore.Mvc;
using mini_store.Models;
using mini_store.Data;

namespace mini_store.Controllers
{
    public class ProductsController:Controller
    {
        private readonly AppDbContext _context  ;

        public ProductsController(AppDbContext cn)
        {
            _context=cn;
        }
         

         public IActionResult Index()
        {
            var products=_context.products.ToList();  // Read All Products
            
            return View(products);
        }


        public IActionResult Create()
        {
            return View();
        }

         
         [HttpPost]

         public IActionResult Create(Product product)
        {
            _context.products.Add(product);  // insert into Products Table 
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

       

    }
}