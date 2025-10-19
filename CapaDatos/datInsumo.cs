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
    public class datInsumo
    {
        #region Singleton
        private static readonly datInsumo _instancia = new datInsumo();
        public static datInsumo Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // Listar todos los insumos
        public List<entInsumo> ListarInsumos()
        {
            SqlCommand cmd = null;
            List<entInsumo> lista = new List<entInsumo>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarInsumos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entInsumo i = new entInsumo
                    {
                        InsumoId = Convert.ToInt32(dr["InsumoId"]),
                        Nombre = dr["Nombre"].ToString(),
                        UnidadMedida = dr["UnidadMedida"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(i);
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

        // Insertar insumo
        public int InsertarInsumo(entInsumo i)
        {
            SqlCommand cmd = null;
            int nuevoId = 0;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarInsumo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", i.Nombre);
                cmd.Parameters.AddWithValue("@UnidadMedida", i.UnidadMedida);

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

        // Editar insumo
        public bool EditarInsumo(entInsumo i)
        {
            SqlCommand cmd = null;
            bool edita = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EditarInsumo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsumoId", i.InsumoId);
                cmd.Parameters.AddWithValue("@Nombre", i.Nombre);
                cmd.Parameters.AddWithValue("@UnidadMedida", i.UnidadMedida);
                cmd.Parameters.AddWithValue("@Estado", i.Estado);
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

        #endregion
    }
}
