using CeragemBuenosAires.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace CeragemBuenosAires.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepositorio _productoRepositorio;

        public ProductosController(ProductoRepositorio productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
        }

        public IActionResult Index()
        {
            var productos = _productoRepositorio.ObtenerProductosActivos();

            return View(productos);
        }
    }
}