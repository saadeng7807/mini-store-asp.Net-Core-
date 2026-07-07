using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mini_store.Data;
using mini_store;
using Microsoft.AspNetCore.Identity;
using mini_store.Models;
using Microsoft.AspNetCore.Http; // تأكد من وجود هذه للـ Middleware

// الحل الجذري: نُعطي الحزمة الخارجية اسماً مستعاراً مخصصاً لمنع التعارض التلقائي
using SwaggerModels = Microsoft.OpenApi.Models; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => 
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddEndpointsApiExplorer();

// إعداد Swagger باستخدام الاسم المستعار الآمن
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() 
    {
        Title = "واجهة برمجة تطبيقات المتجر الإليكتروني -",
        Version = "v1",
        Description = "مستندات وواجهة اختبار الـ API الخاصة بنظام المقاهي المتكامل"
    });

    // الحل الفعال: قصر التوثيق على الـ API Controllers فقط وتجاهل صفحات الـ MVC العادية
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.ActionDescriptor.RouteValues.ContainsKey("controller") && 
               !apiDesc.ActionDescriptor.DisplayName!.Contains("AccountController") &&
               !apiDesc.ActionDescriptor.DisplayName!.Contains("HomeController") &&
               !apiDesc.ActionDescriptor.DisplayName!.Contains("ProductsController") &&
               !apiDesc.ActionDescriptor.DisplayName!.Contains("ProductsDetailsController");
    });
});

builder.Services.AddSession(options =>
{ 
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;             
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "ar", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();

// تفعيل خط أنابيب المعالجة (HTTP Pipeline) 

app.UseRequestLocalization();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();
app.UseSession(); 

// 1. تفعيل نظام المصادقة والصلاحيات (يجب أن يكون هنا)
app.UseAuthentication();
app.UseAuthorization();

// 2. الـ Middleware الذكي لحماية مسار السواجر وإجبار المستخدم على تسجيل الدخول
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/Account/Login");
            return;
        }
    }
    await next();
});

// 3. تشغيل السواجر بعد تخطي فحص تسجيل الدخول
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Store API V1");
        
        // تفعيل حفظ حالة الدخول عند تحديث الصفحة
        options.EnablePersistAuthorization();
        
        // إجبار السواجر على إرسال الكوكيز مع كل طلب داخلي
        options.ConfigObject.AdditionalItems["withCredentials"] = true;
    });
}

app.MapControllers(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();