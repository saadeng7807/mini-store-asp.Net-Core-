using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mini_store.Data;
using mini_store;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => 
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

 builder.Services.AddDbContext<AppDbContext>(Options=>Options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
 ));
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "ar", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
options.SetDefaultCulture(supportedCultures[0]);
options.AddSupportedCultures(supportedCultures);
options.AddSupportedUICultures(supportedCultures);
// إزالة مزود لغة المتصفح
var browserLanguageProvider = options.RequestCultureProviders.OfType<Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider>()
.FirstOrDefault();
if (browserLanguageProvider != null)
{
options.RequestCultureProviders.Remove(browserLanguageProvider);
}
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRequestLocalization();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
