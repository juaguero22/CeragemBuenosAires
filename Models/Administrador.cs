namespace CeragemBuenosAires.Models
{
    public class Administrador
    {
        public int IdAdministrador { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string ContrasenaHash { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}