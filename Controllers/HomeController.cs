using System.Diagnostics;
using System.Linq;
using System;
using CeragemBuenosAires.Models;
using CeragemBuenosAires.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace CeragemBuenosAires.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SolicitudRepositorio _solicitudRepositorio;

        public HomeController(
            ILogger<HomeController> logger,
            SolicitudRepositorio solicitudRepositorio)
        {
            _logger = logger;
            _solicitudRepositorio = solicitudRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View(new Solicitud());
        }
        public IActionResult Nosotros()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contacto(Solicitud solicitud)
        {
            if (string.IsNullOrWhiteSpace(solicitud.NombreCliente) ||
                string.IsNullOrWhiteSpace(solicitud.Telefono) ||
                string.IsNullOrWhiteSpace(solicitud.TipoSolicitud))
            {
                ViewBag.Error = "Completa los campos obligatorios.";
                return View(solicitud);
            }

            string[] tiposPermitidos =
            {
                "Terapias CERAGEM",
                "Masajes y relajación",
                "Cuidado facial",
                "Promociones y otras consultas"
            };

            if (!tiposPermitidos.Contains(solicitud.TipoSolicitud))
            {
                ViewBag.Error = "Selecciona un tipo de consulta válido.";
                return View(solicitud);
            }

            try
            {
                bool guardada =
                    _solicitudRepositorio.GuardarSolicitud(solicitud);

                if (guardada)
                {
                    string mensajeWhatsApp =
                        $"Hola, soy {solicitud.NombreCliente}. " +
                        $"Mi teléfono es {solicitud.Telefono}. " +
                        $"Mi consulta es sobre: {solicitud.TipoSolicitud}. " +
                        $"Mensaje: {solicitud.Mensaje ?? "Sin mensaje adicional."}";

                    string urlWhatsApp =
                        "https://wa.me/50686812402?text=" +
                        Uri.EscapeDataString(mensajeWhatsApp);

                    return Redirect(urlWhatsApp);
                }

                ViewBag.Error = "No fue posible guardar tu solicitud.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar una solicitud.");

                ViewBag.Error =
                    "Ocurrió un error al enviar tu solicitud. Inténtalo nuevamente.";
            }

            return View(solicitud);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                            HttpContext.TraceIdentifier
            });
        }
    }
}