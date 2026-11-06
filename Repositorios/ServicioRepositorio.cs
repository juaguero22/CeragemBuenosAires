using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Models;
using Microsoft.Data.SqlClient;

namespace CeragemBuenosAires.Repositorios
{
    public class ServicioRepositorio
    {
        private readonly ConexionBD _conexionBD;

        public ServicioRepositorio(ConexionBD conexionBD)
        {
            _conexionBD = conexionBD;
        }

        public List<Servicio> ObtenerServiciosActivos()
        {
            var servicios = new List<Servicio>();

            using var conexion = _conexionBD.ObtenerConexion();

            string consulta = @"
                SELECT IdServicio, IdCategoria, Nombre,
                       Descripcion, DuracionMinutos, Precio,
                       Imagen, Estado, FechaRegistro
                FROM Servicios
                WHERE Estado = 1
                ORDER BY Nombre";

            using var comando = new SqlCommand(consulta, conexion);

            conexion.Open();

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                servicios.Add(new Servicio
                {
                    IdServicio = lector.GetInt32(0),
                    IdCategoria = lector.GetInt32(1),
                    Nombre = lector.GetString(2),
                    Descripcion = lector.IsDBNull(3) ? null : lector.GetString(3),
                    DuracionMinutos = lector.IsDBNull(4) ? null : lector.GetInt32(4),
                    Precio = lector.IsDBNull(5) ? null : lector.GetDecimal(5),
                    Imagen = lector.IsDBNull(6) ? null : lector.GetString(6),
                    Estado = lector.GetBoolean(7),
                    FechaRegistro = lector.GetDateTime(8)
                });
            }

            return servicios;
        }

        public Servicio? ObtenerServicioPorId(int id)
        {
            using var conexion = _conexionBD.ObtenerConexion();

            string consulta = @"
        SELECT IdServicio, IdCategoria, Nombre,
               Descripcion, DuracionMinutos, Precio,
               Imagen, Estado, FechaRegistro
        FROM Servicios
        WHERE IdServicio = @IdServicio
          AND Estado = 1";

            using var comando = new SqlCommand(consulta, conexion);

            comando.Parameters.AddWithValue("@IdServicio", id);

            conexion.Open();

            using var lector = comando.ExecuteReader();

            if (lector.Read())
            {
                return new Servicio
                {
                    IdServicio = lector.GetInt32(0),
                    IdCategoria = lector.GetInt32(1),
                    Nombre = lector.GetString(2),
                    Descripcion = lector.IsDBNull(3) ? null : lector.GetString(3),
                    DuracionMinutos = lector.IsDBNull(4) ? null : lector.GetInt32(4),
                    Precio = lector.IsDBNull(5) ? null : lector.GetDecimal(5),
                    Imagen = lector.IsDBNull(6) ? null : lector.GetString(6),
                    Estado = lector.GetBoolean(7),
                    FechaRegistro = lector.GetDateTime(8)
                };
            }

            return null;
        }

    }
}