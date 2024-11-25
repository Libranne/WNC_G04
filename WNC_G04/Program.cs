using WNC_G04.Service;
using Microsoft.EntityFrameworkCore;
using WNC_G04.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbG04RVContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("G04RV")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thiết lập thời gian hết hạn cho Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<IDataService, DataService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // Đừng quên gọi UseSession nếu bạn sử dụng Session

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=LogIn}/{id?}");

app.Run();