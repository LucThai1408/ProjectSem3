using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectSem3.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DoctorContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));
builder.Services.AddDistributedMemoryCache();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication("EprojectSchema").AddCookie("EprojectSchema", options =>
{
    //options.AccessDeniedPath = "/SupperAdmin/Home";
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "Eproject.cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {

    };
    options.LoginPath = new PathString("/Admin/Home/Login");
    options.ReturnUrlParameter = "redirectPath";
    options.SlidingExpiration = true;
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromHours(1);
    option.Cookie.Name = "Eproject.Session";
    option.Cookie.HttpOnly = true;
});

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
