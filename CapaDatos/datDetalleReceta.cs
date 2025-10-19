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
    public class datDetalleReceta
    {
        #region Singleton
        private static readonly datDetalleReceta _instancia = new datDetalleReceta();
        public static datDetalleReceta Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // Listar receta de un producto
        public List<entDetalleReceta> ListarRecetaPorProducto(int productoId)
        {
            SqlCommand cmd = null;
            List<entDetalleReceta> lista = new List<entDetalleReceta>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarRecetaPorProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entDetalleReceta detalle = new entDetalleReceta
                    {
                        ProductoId = Convert.ToInt32(dr["ProductoId"]),
                        InsumoId = Convert.ToInt32(dr["InsumoId"]),
                        NombreInsumo = dr["NombreInsumo"].ToString(),
                        UnidadMedida = dr["UnidadMedida"].ToString(),
                        Cantidad = Convert.ToDecimal(dr["Cantidad"])
                    };
                    lista.Add(detalle);
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

        // Insertar ingrediente a receta
        public bool InsertarDetalleReceta(entDetalleReceta detalle)
        {
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarDetalleReceta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                cmd.Parameters.AddWithValue("@InsumoId", detalle.InsumoId);
                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cn.Open();

                cmd.ExecuteNonQuery();
                resultado = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return resultado;
        }

        // Eliminar ingrediente de receta
        public bool EliminarDetalleReceta(int productoId, int insumoId)
        {
            SqlCommand cmd = null;
            bool elimina = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EliminarDetalleReceta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cmd.Parameters.AddWithValue("@InsumoId", insumoId);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) elimina = true;
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

        // Eliminar receta completa
        public bool EliminarRecetaCompleta(int productoId)
        {
            SqlCommand cmd = null;
            bool elimina = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EliminarRecetaCompleta", cn);
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

        #endregion
    }
}
