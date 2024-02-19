using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NopBookStore.Data;
using NopBookStore.IServices;
using NopBookStore.Middleware;
using NopBookStore.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ModernBookShopDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BSDay15ConnectionString")),ServiceLifetime.Scoped);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<CurrentUserMiddleware>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/User/Login"; // Specify the login page
    options.AccessDeniedPath = "/Home/AccessDenied"; // Specify the access denied page
});

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();


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

// Custom middleware for authentication
app.Use(async (context, next) =>
{
    var userEmail = context.Request.Cookies["UserId"];
    var userPassword = context.Request.Cookies["UserPassword"];

    if (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(userPassword))
    {
        var dbContext = context.RequestServices.GetRequiredService<ModernBookShopDbContext>();
        var userService = context.RequestServices.GetRequiredService<IUserService>();

        // Assuming you have a method in IUserService to authenticate the user
        var user = await userService.AuthenticateUserAsync(userEmail, userPassword);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                // Add more claims as needed
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            context.User = new ClaimsPrincipal(claimsIdentity);
        }
    }

    await next();
});
app.UseAuthorization();
app.UseMiddleware<CurrentUserMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
