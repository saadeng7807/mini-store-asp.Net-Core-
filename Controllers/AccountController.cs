using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mini_store.ViewModels;
using System.Threading.Tasks;
using mini_store.Models;
namespace mini_store.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;  // خاص بعملية تسجيل الدخول

         private readonly UserManager<IdentityUser> _userManager; 

        // 2. حقن الأدوات في المُشيّد (Constructor)
        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager) 
        {
            _signInManager = signInManager;
            _userManager = userManager; 
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "/") 
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // محاولة تسجيل الدخول
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                
                    return RedirectToAction("Index", "Products");
                }
                
             
                ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // تنظيف جلسة الدخول وحذف ملف تعريف الارتباط (Cookie)
            await _signInManager.SignOutAsync();
            
            // توجيه المستخدم إلى الصفحة الرئيسية بعد تسجيل الخروج
            return RedirectToAction("Index", "Home");
        }
       
        [HttpGet]
            public IActionResult Register()
            {
                return View();
            }


     [HttpPost]

     public async Task<IActionResult> Register(RegisterViewModel  register)
        {
            if(ModelState.IsValid)
            {

                    IdentityUser user=new ApplicationUser
                    {
                    
                    UserName=register.Email,
                    Email=register.Email,
                    FirstName=register.FirstName,
                    LastName=register.LastName,
                    };


                    IdentityResult result=await _userManager.CreateAsync(user,register.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }

                
            }


           
            return View(register);
        }

        
          
    }


    
}