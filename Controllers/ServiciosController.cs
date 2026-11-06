using CeragemBuenosAires.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace CeragemBuenosAires.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ServicioRepositorio _servicioRepositorio;

        public ServiciosController(ServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

        public IActionResult Index()
        {
            var servicios = _servicioRepositorio.ObtenerServiciosActivos();

            return View(servicios);
        }

        public IActionResult Detalle(int id)
        {
            var servicio = _servicioRepositorio.ObtenerServicioPorId(id);

            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }
    }
}
