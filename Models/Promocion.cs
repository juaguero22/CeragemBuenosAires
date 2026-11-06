namespace CeragemBuenosAires.Models
{
    public class Promocion
    {
        public int IdPromocion { get; set; }

        public int? IdProducto { get; set; }

        public int? IdServicio { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal? PrecioPromocional { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string? Imagen { get; set; }

        public bool Estado { get; set; }
    }
}
