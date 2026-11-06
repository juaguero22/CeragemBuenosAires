using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Models;
using Microsoft.Data.SqlClient;

namespace CeragemBuenosAires.Repositorios
{
    public class ProductoRepositorio
    {
        private readonly ConexionBD _conexionBD;

        public ProductoRepositorio(ConexionBD conexionBD)
        {
            _conexionBD = conexionBD;
        }

        public List<Producto> ObtenerProductosActivos()
        {
            var productos = new List<Producto>();

            using var conexion = _conexionBD.ObtenerConexion();

            string consulta = @"
                SELECT IdProducto, IdCategoria, Nombre, Marca,
                       Descripcion, Precio, Imagen, Estado,
                       FechaRegistro, Presentacion, Usos
                FROM Productos
                WHERE Estado = 1
                ORDER BY Nombre";

            using var comando = new SqlCommand(consulta, conexion);

            conexion.Open();

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                productos.Add(new Producto
                {
                    IdProducto = lector.GetInt32(0),
                    IdCategoria = lector.GetInt32(1),
                    Nombre = lector.GetString(2),
                    Marca = lector.IsDBNull(3) ? null : lector.GetString(3),
                    Descripcion = lector.IsDBNull(4) ? null : lector.GetString(4),
                    Precio = lector.IsDBNull(5) ? null : lector.GetDecimal(5),
                    Imagen = lector.IsDBNull(6) ? null : lector.GetString(6),
                    Estado = lector.GetBoolean(7),
                    FechaRegistro = lector.GetDateTime(8),
                    Presentacion = lector.IsDBNull(9) ? null : lector.GetString(9),
                    Usos = lector.IsDBNull(10) ? null : lector.GetString(10)
                });
            }

            return productos;
        }
    }
}