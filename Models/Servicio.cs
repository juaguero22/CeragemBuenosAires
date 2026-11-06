namespace CeragemBuenosAires.Models
{
    public class Servicio
    {
        public int IdServicio { get; set; }

        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public int? DuracionMinutos { get; set; }

        public decimal? Precio { get; set; }

        public string? Imagen { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}