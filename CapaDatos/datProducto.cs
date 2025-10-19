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
    public class datProducto
    {
        #region Singleton
        private static readonly datProducto _instancia = new datProducto();
        public static datProducto Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // LISTAR TODOS LOS PRODUCTOS
        public List<entProducto> ListarProductos()
        {
            SqlCommand cmd = null;
            List<entProducto> lista = new List<entProducto>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProducto p = new entProducto
                    {
                        ProductoId = Convert.ToInt32(dr["ProductoId"]),
                        Nombre = dr["Nombre"].ToString(),
                        Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : "",
                        CategoriaId = Convert.ToInt32(dr["CategoriaId"]),
                        NombreCategoria = dr["NombreCategoria"].ToString(),
                        EstacionId = dr["EstacionId"] != DBNull.Value ? Convert.ToInt32(dr["EstacionId"]) : (int?)null,
                        NombreEstacion = dr["NombreEstacion"] != DBNull.Value ? dr["NombreEstacion"].ToString() : "",
                        Imagen = dr["Imagen"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };

                    // Cargar variantes
                    p.Variantes = datProductoVariante.Instancia.ListarVariantesPorProducto(p.ProductoId);

                    lista.Add(p);
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

        // Actualizar InsertarProducto - quitar parámetro Precio
        public int InsertarProducto(entProducto p)
        {
            SqlCommand cmd = null;
            int nuevoId = 0;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", (object)p.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoriaId", p.CategoriaId);
                cmd.Parameters.AddWithValue("@EstacionId", (object)p.EstacionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Imagen", (object)p.Imagen ?? DBNull.Value);

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

        // EDITAR PRODUCTO
        public bool EditarProducto(entProducto p)
        {
            SqlCommand cmd = null;
            bool edita = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EditarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", p.ProductoId);
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", (object)p.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoriaId", p.CategoriaId);
                cmd.Parameters.AddWithValue("@EstacionId", (object)p.EstacionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Imagen", (object)p.Imagen ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", p.Estado);
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

        // LISTAR PRODUCTOS ACTIVOS
        public List<entProducto> ListarProductosActivos()
        {
            SqlCommand cmd = null;
            List<entProducto> lista = new List<entProducto>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarProductosActivos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProducto p = new entProducto
                    {
                        ProductoId = Convert.ToInt32(dr["ProductoId"]),
                        Nombre = dr["Nombre"].ToString(),
                        Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : "",
                        CategoriaId = Convert.ToInt32(dr["CategoriaId"]),
                        NombreCategoria = dr["NombreCategoria"].ToString(),
                        EstacionId = dr["EstacionId"] != DBNull.Value ? Convert.ToInt32(dr["EstacionId"]) : (int?)null,
                        NombreEstacion = dr["NombreEstacion"] != DBNull.Value ? dr["NombreEstacion"].ToString() : "",
                        Imagen = dr["Imagen"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(p);
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

        #endregion
    }
}
