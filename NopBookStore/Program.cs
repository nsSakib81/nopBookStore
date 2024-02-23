using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NopBookStore.Data;
using NopBookStore.IServices;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NopBookStore.Middleware;
using NopBookStore.Services;
using System.Reflection;
using System.Security.Claims;
using NopBookStore.Mapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ModernBookShopDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BSDay15ConnectionString")), ServiceLifetime.Scoped);
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(config =>
        config.AddSqlServer()
        .WithGlobalConnectionString("BSDay15ConnectionString") //FluentMigration Added
        .ScanIn(Assembly.GetExecutingAssembly()).For.All()
        )
    .AddLogging(config => config.AddFluentMigratorConsole());
builder.Services.AddAutoMapper(typeof(MapperProfile));

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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userEmail = context.Request.Cookies["UserId"];
    var userPassword = context.Request.Cookies["UserPassword"];

    if (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(userPassword))
    {
        var dbContext = context.RequestServices.GetRequiredService<ModernBookShopDbContext>();
        var userService = context.RequestServices.GetRequiredService<IUserService>();

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

app.UseMiddleware<CurrentUserMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var serviceProvider = app.Services;
using var scope = app.Services.CreateScope();
var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();

app.Run();
