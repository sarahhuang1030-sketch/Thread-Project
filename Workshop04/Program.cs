using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Workshop04.Data.Data;
using Workshop04.Data.Models;
using Workshop04.Data;
using Workshop04.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//EF core
//builder.Services.AddDbContext<TravelExpertsContext>(options =>
//options.UseSqlServer(
//    builder.Configuration.GetConnectionString("TravelExpertsConnection")));

//builder.Services.AddDbContext<TravelExpertsContext>(options =>
//    options
//        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));



//add full identity
builder.Services.AddIdentity<Customer, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false; // Disable email confirmation for now
})
.AddEntityFrameworkStores<TravelExpertsContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Add Email Sender (No-Op for development)



// Add Email Sender (No-Op for development)
builder.Services.AddTransient<IEmailSender, DevEmailSender>();

// Add Image Upload Service
builder.Services.AddScoped<IImageUploadService, ImageUploadService>();

//sessions
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<TravelExpertsContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLowerCaseNamingConvention()
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

var app = builder.Build();

// Create the database if it doesn't exist
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<TravelExpertsContext>();
//    db.Database.EnsureCreated();
//}




//auto-create/update the SQLite DB on startup
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<TravelExpertsContext>();
//    db.Database.Migrate();
//}

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

app.UseSession();
app.UseAuthentication();   // identity cookie auth
app.UseAuthorization();

// Map Razor Pages first (Identity pages)
app.MapRazorPages();

// Then map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

Directory.CreateDirectory("/var/data");

app.Run();
