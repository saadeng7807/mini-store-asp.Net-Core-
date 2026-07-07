using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using mini_store.Models;
using mini_store.ViewModels;
using mini_store.Data;
using Microsoft.EntityFrameworkCore;


public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // دالة إضافة المنتج إلى السلة
    public IActionResult AddToCart(int id)
    {
       
        var Count=HttpContext.Session.GetInt32("counter") ?? 0; 

        Count++;

        HttpContext.Session.SetInt32("counter", Count);
        
        TempData["SuccessMessage"] = "تمت إضافة المنتج إلى السلة بنجاح!";

        return RedirectToAction("Details", "Home", new { id = id });
    }
}