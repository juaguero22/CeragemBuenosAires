using System.Security.Claims;
using CeragemBuenosAires.Models;
using CeragemBuenosAires.Repositorios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CeragemBuenosAires.Controllers
{
    public class AdministradoresController : Controller
    {
        private readonly AdministradorRepositorio _repositorio;
        private readonly IPasswordHasher<Administrador> _passwordHasher;

        public AdministradoresController(
            AdministradorRepositorio repositorio,
            IPasswordHasher<Administrador> passwordHasher)
        {
            _repositorio = repositorio;
            _passwordHasher = passwordHasher;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Error = "Ingresa tu correo y contraseña.";
                return View();
            }

            var administrador = _repositorio.ObtenerPorCorreo(correo.Trim());

            if (administrador == null || !administrador.Estado)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                administrador,
                administrador.ContrasenaHash,
                contrasena
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    administrador.IdAdministrador.ToString()),

                new Claim(ClaimTypes.Name, administrador.Nombre),

                new Claim(ClaimTypes.Email, administrador.Correo)
            };

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Administradores");
        }

        [Authorize]
        public IActionResult Index()
        {
            return View("Panel");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login");
        }
    }
}