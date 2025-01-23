using Microsoft.EntityFrameworkCore;
using TestGp.Controllers;
using TestGp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<EmailService>();

builder.Services.AddTransient<EventsController>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(30));
builder.Services.AddDbContext<Mydb>(options =>
{
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBCONTEXTTT1 ;Integrated Security=True");
});

var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();