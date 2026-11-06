
using Microsoft.Data.SqlClient;

namespace CeragemBuenosAires.Datos
{
    public class ConexionBD
    {
        private readonly IConfiguration _configuration;

        public ConexionBD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection ObtenerConexion()
        {
            string cadenaConexion = _configuration.GetConnectionString("ConexionSQL")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión ConexionSQL.");

            return new SqlConnection(cadenaConexion);
        }
    }
}
