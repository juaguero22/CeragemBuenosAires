using CeragemBuenosAires.Datos;
using CeragemBuenosAires.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CeragemBuenosAires.Repositorios
{
    public class SolicitudRepositorio
    {
        private readonly ConexionBD _conexionBD;

        public SolicitudRepositorio(ConexionBD conexionBD)
        {
            _conexionBD = conexionBD;
        }

        public bool GuardarSolicitud(Solicitud solicitud)
        {
            using var conexion = _conexionBD.ObtenerConexion();

            string consulta = @"
                INSERT INTO Solicitudes
                (
                    NombreCliente,
                    Telefono,
                    Correo,
                    TipoSolicitud,
                    Mensaje,
                    FechaSolicitud,
                    Estado
                )
                VALUES
                (
                    @NombreCliente,
                    @Telefono,
                    @Correo,
                    @TipoSolicitud,
                    @Mensaje,
                    GETDATE(),
                    'Pendiente'
                )";

            using var comando = new SqlCommand(consulta, conexion);

            comando.Parameters.Add("@NombreCliente", SqlDbType.VarChar, 120)
                .Value = solicitud.NombreCliente;

            comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 20)
                .Value = solicitud.Telefono;

            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 120)
                .Value = (object?)solicitud.Correo ?? DBNull.Value;

            comando.Parameters.Add("@TipoSolicitud", SqlDbType.VarChar, 60)
                .Value = solicitud.TipoSolicitud;

            comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500)
                .Value = (object?)solicitud.Mensaje ?? DBNull.Value;

            conexion.Open();

            return comando.ExecuteNonQuery() > 0;
        }
    }
}
