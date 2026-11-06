using CeragemBuenosAires.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace CeragemBuenosAires.Controllers
{
    public class PromocionesController : Controller
    {
        private readonly PromocionRepositorio _promocionRepositorio;

        public PromocionesController(PromocionRepositorio promocionRepositorio)
        {
            _promocionRepositorio = promocionRepositorio;
        }

        public IActionResult Index()
        {
            var promociones = _promocionRepositorio.ObtenerPromocionesActivas();

            return View(promociones);
        }
    }
}
