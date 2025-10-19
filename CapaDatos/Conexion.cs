using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class Conexion
    {
        #region Singleton
        private static readonly Conexion UnicaInstancia = new Conexion();
        public static Conexion Instancia
        {
            get
            {
                return Conexion.UnicaInstancia;
            }

        }
        #endregion Singleton

        #region Metodos
        public SqlConnection Conectar()
        {

            SqlConnection cn = new SqlConnection();

            cn.ConnectionString = "Data Source=localhost\\SQLEXPRESS;initial Catalog=Casa_Blanca;" + "Integrated Security=true;TrustServerCertificate=True ";

            return cn;
        }
        #endregion Metodos
    }
}
