using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentCars;
using RentCars.BackgroundServices;
using RentCars.Data;
using RentCars.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {

    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
    {
        policy.RequireRole("admin");
    });
    options.AddPolicy("RequireUserRole", policy =>
    {
        policy.RequireRole("user");
    });
    options.AddPolicy("RequireAdminOrUserRole", policy =>
    {
        policy.RequireRole("admin", "user");
    });

});

builder.Services.AddSingleton<BackgroundDateService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<BackgroundDateService>());
builder.Services.AddControllersWithViews();

//opisac
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRentService, RentService>();
builder.Services.AddScoped<IReservationService, ReservationService>();


var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ApplicationMappingProfile());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

#region
SeedDataBase();

void SeedDataBase()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var scopedContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Seeder.Seed(scopedContext);
        }
        catch
        {
            throw;
        }
    }
}
#endregion




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
