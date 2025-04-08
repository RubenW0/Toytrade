using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Repositorys;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.IRepositorys;
using BusinessLogicLayer.DTOs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Test database verbinding
Console.WriteLine($"🔍 Connection String: {connectionString}");
try
{
    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("✅ Verbinding met de database is geslaagd!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Databasefout: {ex.Message}");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

// Dependency Injection
builder.Services.AddScoped<IToyRepository, ToyRepository>(); // Interface koppelen aan implementatie
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Interface koppelen aan implementatie

builder.Services.AddScoped<ToyService>(); // Service registreren
builder.Services.AddScoped<UserService>(); // Service registreren

// Register IPasswordHasher<UserDTO>
builder.Services.AddScoped<IPasswordHasher<UserDTO>, PasswordHasher<UserDTO>>();

// Add session
builder.Services.AddSession();

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
