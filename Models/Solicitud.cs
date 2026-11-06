namespace CeragemBuenosAires.Models
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }

        public int? IdProducto { get; set; }

        public int? IdServicio { get; set; }

        public string NombreCliente { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string? Correo { get; set; }

        public string TipoSolicitud { get; set; } = string.Empty;

        public string? Mensaje { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}