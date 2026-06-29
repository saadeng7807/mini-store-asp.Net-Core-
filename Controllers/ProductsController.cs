using Microsoft.AspNetCore.Mvc;
using mini_store.Models;
using mini_store.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.Pkcs;

namespace mini_store.Controllers
{
    public class ProductsController:Controller
    {
        private readonly AppDbContext _context  ;

        public readonly IWebHostEnvironment _webhostingEnvorment;

        public ProductsController(AppDbContext cn, IWebHostEnvironment wse)
        {
            _context=cn;
            _webhostingEnvorment=wse;

        }
         
           
           [Route("dashboard")]
         public IActionResult Index(string searchTerm)
        {
           
            var categories=_context.categories.ToList();
            ViewBag.categories=categories;

            var Productquery=_context.products.AsQueryable();
             
             if(!string.IsNullOrEmpty(searchTerm))
            {
                Productquery=Productquery.Where(p=>p.Name.Contains(searchTerm));
            }

            
             var products=Productquery.ToList();

             ViewBag.CurrentSearch=searchTerm;
            return View(products);
        }


        public IActionResult Create()
        {
            return View();
        }

         
         [HttpPost]

         public IActionResult Create(Product product)
        {
            
            
      
            if(product.ImageFile !=null)
            {
                string UploadFloder=Path.Combine(_webhostingEnvorment.WebRootPath,"Images");

                if(!Directory.Exists(UploadFloder))
                {
                    Directory.CreateDirectory(UploadFloder);
                }
               
                string extention=Path.GetExtension(product.ImageFile.FileName);
                
                string UniqueFileName=Guid.NewGuid().ToString() + extention;    

                string Filepath=Path.Combine(UploadFloder,UniqueFileName)   ; 

                product.Images= UniqueFileName ;
                 ModelState.Remove(nameof(Product.Images));
                using(var filestrame=new FileStream(Filepath,FileMode.Create))
                {
                    product.ImageFile.CopyTo(filestrame);
                }


            }
            

             foreach (var item in ModelState)
{
    foreach (var error in item.Value.Errors)
    {
        Console.WriteLine($"Field: {item.Key}");
        Console.WriteLine($"Error: {error.ErrorMessage}");
    }
}
        
            if(ModelState.IsValid)
            {
                  _context.products.Add(product);  // insert into Products Table 
                  _context.SaveChanges();

                  return RedirectToAction("Index");
            }
           
            ViewBag.categories=_context.categories.ToList();
        
            return View("Index",_context.products.ToList());
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