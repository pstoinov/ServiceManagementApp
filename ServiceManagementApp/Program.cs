//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Interfaces;
using ServiceManagementApp.Services;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Добавете конзолно логване
builder.Logging.AddDebug(); // Добавете логване за дебъг

// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//services.AddDefaultIdentity<IdentityUser>()
//.AddEntityFrameworkStores<ApplicationDbContext>();

// БАЗА ДАННИ ЗА РАЗРАБОТКА
//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseInMemoryDatabase("TestDatabase")); // Използва in-memory база данни в development среда
//}
//else
//{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//}

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddControllers();
builder.Services.AddLogging();

var app = builder.Build();

var scope = app.Services.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

string adminRole = "Admin";
string adminEmail = "admin@example.com";
string adminPassword = "Admin123!";

// Проверка дали ролята "Admin" съществува и създаване, ако не съществува
if (!await roleManager.RoleExistsAsync(adminRole))
{
    await roleManager.CreateAsync(new IdentityRole(adminRole));
}

// Проверка дали администраторският потребител съществува и създаване, ако не съществува
var adminUser = await userManager.FindByEmailAsync(adminEmail);
if (adminUser == null)
{
    adminUser = new IdentityUser
    {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true,
    };
    await userManager.CreateAsync(adminUser, adminPassword);
    await userManager.AddToRoleAsync(adminUser, adminRole);
}

// middleware за обработка на exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();  // За API контролери
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.MapRazorPages();

app.Run();

