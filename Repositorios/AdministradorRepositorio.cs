using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Models;
using Microsoft.Data.SqlClient;

namespace CeragemBuenosAires.Repositorios
{
    public class AdministradorRepositorio
    {
        private readonly ConexionBD _conexionBD;

        public AdministradorRepositorio(ConexionBD conexionBD)
        {
            _conexionBD = conexionBD;
        }

        public Administrador? ObtenerPorCorreo(string correo)
        {
            using var conexion = _conexionBD.ObtenerConexion();

            string consulta = @"
                SELECT IdAdministrador, Nombre, Correo,
                       ContrasenaHash, Estado, FechaRegistro
                FROM Administradores
                WHERE Correo = @Correo";

            using var comando = new SqlCommand(consulta, conexion);

            comando.Parameters.AddWithValue("@Correo", correo);

            conexion.Open();

            using var lector = comando.ExecuteReader();

            if (lector.Read())
            {
                return new Administrador
                {
                    IdAdministrador = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Correo = lector.GetString(2),
                    ContrasenaHash = lector.GetString(3),
                    Estado = lector.GetBoolean(4),
                    FechaRegistro = lector.GetDateTime(5)
                };
            }

            return null;
        }
    }
}