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
    public class datMesa
    {
        #region Singleton
        private static readonly datMesa _instancia = new datMesa();
        public static datMesa Instancia
        {
            get { return _instancia; }
        }
        #endregion

        // Listar todas las Mesas activas
        public List<entMesa> ListarMesas()
        {
            SqlCommand cmd = null;
            List<entMesa> lista = new List<entMesa>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarMesas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entMesa m = new entMesa
                    {
                        MesaId = Convert.ToInt32(dr["MesaId"]),
                        NroMesa = Convert.ToInt32(dr["NroMesa"]),
                        Estado = dr["Estado"].ToString()
                    };
                    lista.Add(m);
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

        // Listar todas las Mesas (incluye eliminadas)
        public List<entMesa> ListarMesasTodo()
        {
            SqlCommand cmd = null;
            List<entMesa> lista = new List<entMesa>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarMesasTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entMesa m = new entMesa
                    {
                        MesaId = Convert.ToInt32(dr["MesaId"]),
                        NroMesa = Convert.ToInt32(dr["NroMesa"]),
                        Estado = dr["Estado"].ToString()
                    };
                    lista.Add(m);
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

        // Insertar Mesa
        public int InsertarMesa(entMesa mesa)
        {
            SqlCommand cmd = null;
            int nuevoId = 0;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarMesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NroMesa", mesa.NroMesa);

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

        // Actualizar Mesa
        public bool ActualizarMesa(entMesa mesa)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarMesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MesaId", mesa.MesaId);
                cmd.Parameters.AddWithValue("@NroMesa", mesa.NroMesa);
                cmd.Parameters.AddWithValue("@Estado", mesa.Estado);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }

        // Eliminar Mesa (lógica)
        public bool EliminarMesa(int mesaId)
        {
            SqlCommand cmd = null;
            bool elimina = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EliminarMesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MesaId", mesaId);
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
    }
}
