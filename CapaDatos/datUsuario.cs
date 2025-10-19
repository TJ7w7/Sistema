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
    public class datUsuario
    {
        #region Singleton
        private static readonly datUsuario _instancia = new datUsuario();
        public static datUsuario Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // Listar todos los usuarios
        public List<entUsuario> ListarUsuarios()
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarUsuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario u = new entUsuario
                    {
                        UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                        Rol = dr["Rol"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        Pass = dr["Pass"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"]),
                        EstacionId = dr["EstacionId"] != DBNull.Value ? Convert.ToInt32(dr["EstacionId"]) : (int?)null,
                        EstacionNombre = dr["NombreEstacion"] != DBNull.Value ? dr["EstacionNombre"].ToString() : null
                    };
                    lista.Add(u);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ListarUsuarios: {ex.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        // Insertar nuevo usuario
        public bool InsertarUsuario(entUsuario u)
        {
            SqlCommand cmd = null;
            bool inserta = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_InsertarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Rol", u.Rol);
                cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                cmd.Parameters.AddWithValue("@UserName", u.UserName);
                cmd.Parameters.AddWithValue("@Pass", u.Pass);

                if (u.EstacionId.HasValue)
                    cmd.Parameters.AddWithValue("@EstacionId", u.EstacionId);
                else
                    cmd.Parameters.AddWithValue("@EstacionId", DBNull.Value);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                inserta = i > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en InsertarUsuario: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return inserta;
        }

        // Editar usuario existente
        public bool EditarUsuario(entUsuario u)
        {
            SqlCommand cmd = null;
            bool edita = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UsuarioId", u.UsuarioId);
                cmd.Parameters.AddWithValue("@Rol", u.Rol);
                cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                cmd.Parameters.AddWithValue("@UserName", u.UserName);
                cmd.Parameters.AddWithValue("@Pass", u.Pass);
                cmd.Parameters.AddWithValue("@Estado", u.Estado);

                if (u.EstacionId.HasValue)
                    cmd.Parameters.AddWithValue("@EstacionId", u.EstacionId);
                else
                    cmd.Parameters.AddWithValue("@EstacionId", DBNull.Value);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                edita = i > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en EditarUsuario: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return edita;
        }

        // Obtener usuario por Id
        public entUsuario ObtenerUsuarioPorId(int usuarioId)
        {
            SqlCommand cmd = null;
            entUsuario u = null;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ObtenerUsuarioPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        u = new entUsuario
                        {
                            UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                            Rol = dr["Rol"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            UserName = dr["UserName"].ToString(),
                            Pass = dr["Pass"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"]),
                            EstacionId = dr["EstacionId"] != DBNull.Value ? Convert.ToInt32(dr["EstacionId"]) : (int?)null,
                            EstacionNombre = null
                        };

                        // Leer nombre de estación intentando dos alias posibles
                        try
                        {
                            int idx = dr.GetOrdinal("EstacionNombre");
                            u.EstacionNombre = dr.IsDBNull(idx) ? null : dr.GetString(idx);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            try
                            {
                                int idx2 = dr.GetOrdinal("NombreEstacion");
                                u.EstacionNombre = dr.IsDBNull(idx2) ? null : dr.GetString(idx2);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                // columna no presente, dejar null
                                u.EstacionNombre = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerUsuarioPorId: {ex.Message}");
                throw ex;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                    cmd.Connection.Close();
            }

            return u;
        }

        public bool ExisteNombreUsuario(string userName)
        {
            SqlCommand cmd = null;
            bool existe = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ExisteNombreUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", userName);

                cn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int valor = Convert.ToInt32(result);
                    existe = (valor == 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar si existe el nombre de usuario", ex);
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                    cmd.Connection.Close();
            }

            return existe;
        }
        #endregion

        #region Método para Autenticación
        public entUsuario AutenticarUsuario(string userName, string passwordEncriptado)
        {
            SqlCommand cmd = null;
            entUsuario u = null;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_AutenticarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Pass", passwordEncriptado);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        u = new entUsuario
                        {
                            UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                            Rol = dr["Rol"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            UserName = dr["UserName"].ToString(),
                            Pass = dr["Pass"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"]),
                            EstacionId = dr["EstacionId"] != DBNull.Value ? Convert.ToInt32(dr["EstacionId"]) : (int?)null
                        };

                        // Obtener nombre de estación si existe
                        try
                        {
                            int idx = dr.GetOrdinal("EstacionNombre");
                            u.EstacionNombre = dr.IsDBNull(idx) ? null : dr.GetString(idx);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            try
                            {
                                int idx2 = dr.GetOrdinal("NombreEstacion");
                                u.EstacionNombre = dr.IsDBNull(idx2) ? null : dr.GetString(idx2);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                u.EstacionNombre = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en AutenticarUsuario: {ex.Message}");
                throw;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                    cmd.Connection.Close();
            }

            return u;
        }
        #endregion
    }
}
