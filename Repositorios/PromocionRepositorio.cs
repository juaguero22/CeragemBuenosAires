using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Models;
using Microsoft.Data.SqlClient;

namespace CeragemBuenosAires.Repositorios
{
    public class PromocionRepositorio
    {
        private readonly ConexionBD _conexionBD;

        public PromocionRepositorio(ConexionBD conexionBD)
        {
            _conexionBD = conexionBD;
        }

        public List<Promocion> ObtenerPromocionesActivas()
        {
            var promociones = new List<Promocion>();

            using var conexion = _conexionBD.ObtenerConexion();

            string consulta = @"
                SELECT IdPromocion, IdProducto, IdServicio,
                       Titulo, Descripcion, PrecioPromocional,
                       FechaInicio, FechaFin, Imagen, Estado
                FROM Promociones
                WHERE Estado = 1
                ORDER BY IdPromocion DESC";

            using var comando = new SqlCommand(consulta, conexion);

            conexion.Open();

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                promociones.Add(new Promocion
                {
                    IdPromocion = lector.GetInt32(0),
                    IdProducto = lector.IsDBNull(1) ? null : lector.GetInt32(1),
                    IdServicio = lector.IsDBNull(2) ? null : lector.GetInt32(2),
                    Titulo = lector.GetString(3),
                    Descripcion = lector.IsDBNull(4) ? null : lector.GetString(4),
                    PrecioPromocional = lector.IsDBNull(5) ? null : lector.GetDecimal(5),
                    FechaInicio = lector.IsDBNull(6) ? null : lector.GetDateTime(6),
                    FechaFin = lector.IsDBNull(7) ? null : lector.GetDateTime(7),
                    Imagen = lector.IsDBNull(8) ? null : lector.GetString(8),
                    Estado = lector.GetBoolean(9)
                });
            }

            return promociones;
        }
    }
}