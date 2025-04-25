using Virtualshop.Web.Services;
using Virtualshop.Web.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductService, ProductService>("ProductApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:ProductApi"]);
    c.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
    c.DefaultRequestHeaders.Add("Keep-Alive", "3600");
    c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-ProductApi");
});

builder.Services.AddHttpClient<ICartService, CartService>("CartApi",
    c => c.BaseAddress = new Uri(builder.Configuration["ServiceUri:CartApi"])
);

builder.Services.AddHttpClient<ICouponService, CouponService>("DiscountApi", c =>
   c.BaseAddress = new Uri(builder.Configuration["ServiceUri:DiscountApi"])
);

builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();