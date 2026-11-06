namespace CeragemBuenosAires.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }

        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Marca { get; set; }

        public string? Descripcion { get; set; }

        public decimal? Precio { get; set; }

        public string? Imagen { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string? Presentacion { get; set; }

        public string? Usos { get; set; }
    }
}
