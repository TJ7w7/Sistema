using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using CapaEntidad;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datEstacion
    {
        #region Singleton
        private static readonly datEstacion _instancia = new datEstacion();
        public static datEstacion Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        public List<entEstacion> ListarEstaciones()
        {
            SqlCommand cmd = null;
            List<entEstacion> lista = new List<entEstacion>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarEstaciones", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entEstacion e = new entEstacion
                    {
                        EstacionId = Convert.ToInt32(dr["EstacionId"]),
                        Nombre = dr["Nombre"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(e);
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

        public bool InsertarEstacion(entEstacion e)
        {
            SqlCommand cmd = null;
            bool inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarEstacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", e.Nombre);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                inserta = i > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en InsertarEstacion: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return inserta;
        }

        public bool EditarEstacion(entEstacion e)
        {
            SqlCommand cmd = null;
            bool edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarEstacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstacionId", e.EstacionId);
                cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                cmd.Parameters.AddWithValue("@Estado", e.Estado);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                edita = i > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en InsertarEstacion: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return edita;
        }

        public List<entEstacion> ListarEstacionesActivas()
        {
            List<entEstacion> lista = new List<entEstacion>();
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarEstacionesActivas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    entEstacion e = new entEstacion();
                    e.EstacionId = Convert.ToInt32(dr["EstacionId"]);
                    e.Nombre = dr["Nombre"].ToString();
                    e.Estado = Convert.ToBoolean(dr["Estado"]);
                    lista.Add(e);
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


        //public bool EliminarEstacion(int id)
        //{
        //    SqlCommand cmd = null;
        //    bool elimina = false;
        //    try
        //    {
        //        SqlConnection cn = Conexion.Instancia.Conectar();
        //        cmd = new SqlCommand("sp_EliminarEstacion", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@EstacionId", id);

        //        cn.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        elimina = i > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        cmd.Connection.Close();
        //    }
        //    return elimina;
        //}

        #endregion
    }
}
