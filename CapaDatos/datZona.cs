using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datZona
    {
        #region Singleton
        private static readonly datZona _instancia = new datZona();
        public static datZona Instancia
        {
            get { return _instancia; }
        }
        #endregion

        public List<entZona> ListarZonas()
        {
            SqlCommand cmd = null;
            List<entZona> lista = new List<entZona>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarZonas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entZona z = new entZona
                    {
                        ZonaId = Convert.ToInt32(dr["ZonaId"]),
                        Nombre = dr["Nombre"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(z);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return lista;
        }

        public int InsertarZona(entZona zona)
        {
            SqlCommand cmd = null;
            int nuevoId = 0;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarZona", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", zona.Nombre);

                SqlParameter paramNuevoId = new SqlParameter("@NuevoId", SqlDbType.Int);
                paramNuevoId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramNuevoId);

                cn.Open();
                cmd.ExecuteNonQuery();
                nuevoId = Convert.ToInt32(paramNuevoId.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return nuevoId;
        }

        public bool EditarZona(entZona zona)
        {
            SqlCommand cmd = null;
            bool edita = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EditarZona", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ZonaId", zona.ZonaId);
                cmd.Parameters.AddWithValue("@Nombre", zona.Nombre);
                cmd.Parameters.AddWithValue("@Estado", zona.Estado);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) edita = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return edita;
        }
    }
}
