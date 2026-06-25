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
         
           
           [Route("dashboard")]
         public IActionResult Index()
        {
            var products=_context.products.ToList();  // Read All Products
            var categories=_context.categories.ToList();
            ViewBag.categories=categories;
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

       public IActionResult Delete(int id)
        {
            var products=_context.products.Find(id);

            if(products !=null)
            {
                _context.products.Remove(products);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
              var products=_context.products.Find(id);
               var categories=_context.categories.ToList();
            ViewBag.categories=categories;
               if(products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("index");


        }
    }
}