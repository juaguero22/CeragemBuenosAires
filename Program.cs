using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Repositorios;
using Microsoft.AspNetCore.Authentication.Cookies;
using CeragemBuenosAires.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Servicios MVC
builder.Services.AddControllersWithViews();

// Conexión a SQL Server
builder.Services.AddScoped<ConexionBD>();

// Repositorios
builder.Services.AddScoped<ServicioRepositorio>();
builder.Services.AddScoped<ProductoRepositorio>();
builder.Services.AddScoped<PromocionRepositorio>();
builder.Services.AddScoped<SolicitudRepositorio>();
builder.Services.AddScoped<AdministradorRepositorio>();
builder.Services.AddScoped<IPasswordHasher<Administrador>, PasswordHasher<Administrador>>();

// Autenticación mediante cookies
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Administradores/Login";
        options.AccessDeniedPath = "/Administradores/Login";
        options.Cookie.Name = "CeragemBuenosAires.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// Configuración de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();