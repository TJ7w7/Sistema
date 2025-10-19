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
    public class datCategoria
    {
        #region Singleton
        private static readonly datCategoria _instancia = new datCategoria();
        public static datCategoria Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // 🔹 Listar todas las categorías
        public List<entCategoria> ListarCategorias()
        {
            SqlCommand cmd = null;
            List<entCategoria> lista = new List<entCategoria>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarCategorias", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entCategoria c = new entCategoria
                    {
                        CategoriaId = Convert.ToInt32(dr["CategoriaId"]),
                        Nombre = dr["Nombre"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(c);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        // 🔹 Insertar nueva categoría
        public bool InsertarCategoria(entCategoria c)
        {
            SqlCommand cmd = null;
            bool inserta = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                inserta = i > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en InsertarCategoria: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return inserta;
        }

        // 🔹 Editar categoría
        public bool EditarCategoria(entCategoria c)
        {
            SqlCommand cmd = null;
            bool edita = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaId", c.CategoriaId);
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Estado", c.Estado);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                edita = i > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en EditarCategoria: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return edita;
        }

        // 🔹 Eliminar categoría (por ID)
        //public bool EliminarCategoria(int idCategoria)
        //{
        //    SqlCommand cmd = null;
        //    bool elimina = false;

        //    try
        //    {
        //        SqlConnection cn = Conexion.Instancia.Conectar();
        //        cmd = new SqlCommand("sp_EliminarCategoria", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@CategoriaId", idCategoria);

        //        cn.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        elimina = i > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error en EliminarCategoria: {ex.Message}");
        //        System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
        //        throw ex;
        //    }
        //    finally
        //    {
        //        cmd.Connection.Close();
        //    }
        //    return elimina;
        //}

        // 🔹 Listar categorías activas
        public List<entCategoria> ListarCategoriasActivas()
        {
            List<entCategoria> lista = new List<entCategoria>();
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarCategoriasActivas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    entCategoria c = new entCategoria();
                    c.CategoriaId = Convert.ToInt32(dr["CategoriaId"]);
                    c.Nombre = dr["Nombre"].ToString();
                    c.Estado = Convert.ToBoolean(dr["Estado"]);
                    lista.Add(c);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
