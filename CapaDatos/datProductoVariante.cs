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
    public class datProductoVariante
    {
        #region Singleton
        private static readonly datProductoVariante _instancia = new datProductoVariante();
        public static datProductoVariante Instancia
        {
            get { return _instancia; }
        }
        #endregion

        public bool InsertarVariante(entProductoVariante v)
        {
            SqlCommand cmd = null;
            bool inserta = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarVariante", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", v.ProductoId);
                cmd.Parameters.AddWithValue("@Tamaño", v.Tamaño);
                cmd.Parameters.AddWithValue("@Precio", v.Precio);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) inserta = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return inserta;
        }

        public List<entProductoVariante> ListarVariantesPorProducto(int productoId)
        {
            SqlCommand cmd = null;
            List<entProductoVariante> lista = new List<entProductoVariante>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarVariantesPorProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProductoVariante v = new entProductoVariante
                    {
                        VarianteId = Convert.ToInt32(dr["VarianteId"]),
                        ProductoId = Convert.ToInt32(dr["ProductoId"]),
                        Tamaño = dr["Tamaño"].ToString(),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(v);
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

        public bool EliminarVariantesPorProducto(int productoId)
        {
            SqlCommand cmd = null;
            bool elimina = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EliminarVariantesPorProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cn.Open();

                cmd.ExecuteNonQuery();
                elimina = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return elimina;
        }
    }
}
