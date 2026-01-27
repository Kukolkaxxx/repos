using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pogodnik;
using Pogodnik.Data;
using Pogodnik.Data.Entities;
using Pogodnik.Data.Repositories;
using Pogodnik.Services;
using Pogodnik.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Добавляем Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Настройки сессии (для хранения UserSessionId)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Регистрация репозиториев
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

// Регистрация сервисов
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddHttpContextAccessor();

// Добавляем остальные сервисы
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// Настройка API погоды
builder.Services.Configure<WeatherApiSettings>(
    builder.Configuration.GetSection("OpenWeatherMap"));

// Регистрация наших сервисов
builder.Services.AddScoped<IWeatherService, OpenWeatherMapService>();

var app = builder.Build();

// Миграция базы данных при запуске и инициализация данных
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    // Инициализируем данные
    await SeedData.Initialize(scope.ServiceProvider);

    // Создание ролей если их нет
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Конвейер обработки HTTP-запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Добавляем middleware для сессий
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Weather}/{action=Index}/{id?}");

app.Run();