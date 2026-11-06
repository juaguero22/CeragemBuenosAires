using System.Data;
using System.Net;
using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CeragemBuenosAires.Controllers
{
    public class ConfiguracionInicialController : Controller
    {
        private readonly ConexionBD _conexionBD;
        private readonly IPasswordHasher<Administrador> _hasher;
        private readonly IWebHostEnvironment _environment;

        private readonly string[] correosPermitidos =
        {
            "chiviss-19-75@hotmail.com",
            "transmuttadigital@gmail.com"
        };

        public ConfiguracionInicialController(
            ConexionBD conexionBD,
            IPasswordHasher<Administrador> hasher,
            IWebHostEnvironment environment)
        {
            _conexionBD = conexionBD;
            _hasher = hasher;
            _environment = environment;
        }

        private bool AccesoPermitido()
        {
            var ip = HttpContext.Connection.RemoteIpAddress;

            return _environment.IsDevelopment()
                && ip != null
                && IPAddress.IsLoopback(ip);
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!AccesoPermitido())
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(
            string nombre,
            string correo,
            string contrasena)
        {
            if (!AccesoPermitido())
                return NotFound();

            correo = correo?.Trim().ToLowerInvariant() ?? "";

            if (!correosPermitidos.Contains(correo))
                return BadRequest("Correo no autorizado.");

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(contrasena) ||
                contrasena.Length < 12)
            {
                ViewBag.Error = "Completa los datos. La contraseña debe tener al menos 12 caracteres.";
                return View();
            }

            using var conexion = _conexionBD.ObtenerConexion();
            conexion.Open();

            using var verificar = new SqlCommand(@"
    SELECT
        COUNT(*) AS Total,
        SUM(CASE WHEN Correo = @Correo THEN 1 ELSE 0 END) AS Existe
    FROM Administradores", conexion);

            verificar.Parameters.Add("@Correo", SqlDbType.VarChar, 120)
                .Value = correo;

            using var lector = verificar.ExecuteReader();
            lector.Read();

            int total = lector.GetInt32(0);
            int existe = lector.IsDBNull(1) ? 0 : lector.GetInt32(1);

            lector.Close();

            if (existe > 0)
            {
                return Content("Este administrador ya está registrado.");
            }

            if (total >= 2)
            {
                return Content("Las dos cuentas administrativas ya fueron creadas.");
            }

            var administrador = new Administrador
            {
                Nombre = nombre,
                Correo = correo,
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            string hash = _hasher.HashPassword(
                administrador, contrasena);

            string consulta = @"
                INSERT INTO Administradores
                (Nombre, Correo, ContrasenaHash, Estado, FechaRegistro)
                VALUES
                (@Nombre, @Correo, @Hash, 1, GETDATE())";

            using var comando = new SqlCommand(consulta, conexion);

            comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 100)
                .Value = nombre;

            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 120)
                .Value = correo;

            comando.Parameters.Add("@Hash", SqlDbType.VarChar, 255)
                .Value = hash;

            comando.ExecuteNonQuery();

            return Content("Administrador registrado correctamente.");
        }
    }
}