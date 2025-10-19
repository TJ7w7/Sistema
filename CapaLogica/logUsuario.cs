using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    //pasar a la capa entidad
    public class ResultadoRegistro
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public string PasswordTemporal { get; set; }
        public string UserName { get; set; }
    }

    public class logUsuario
    {
        #region Singleton
        private static readonly logUsuario _instancia = new logUsuario();
        public static logUsuario Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        public List<entUsuario> ListarUsuarios()
        {
            try
            {
                return datUsuario.Instancia.ListarUsuarios();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios", ex);
            }
        }

        public entUsuario ObtenerUsuarioPorId(int id)
        {
            try
            {
                return datUsuario.Instancia.ObtenerUsuarioPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener usuario por ID", ex);
            }
        }

        public ResultadoRegistro RegistrarUsuario(entUsuario u)
        {
            try
            {
                if (u == null)
                    throw new ArgumentNullException(nameof(u), "El usuario no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(u.Nombre) || string.IsNullOrWhiteSpace(u.Apellido))
                    throw new ArgumentException("El nombre y apellido son obligatorios.");

                // Generar nombre de usuario único
                u.UserName = Recursos.GenerarNombreUsuarioUnico(u.Nombre, u.Apellido);

                // Generar contraseña temporal
                string claveTemporal = Guid.NewGuid().ToString("N").Substring(0, 8);

                // Encriptar la contraseña para guardarla
                u.Pass = Recursos.EncriptarSHA256(claveTemporal);

                // Registrar en la base de datos
                bool resultado = datUsuario.Instancia.InsertarUsuario(u);

                if (resultado)
                {
                    System.Diagnostics.Debug.WriteLine($"Usuario creado: {u.UserName} | Clave: {claveTemporal}");

                    return new ResultadoRegistro
                    {
                        Exito = true,
                        Mensaje = "Usuario registrado correctamente",
                        PasswordTemporal = claveTemporal,
                        UserName = u.UserName // ⭐ Devolver el userName generado
                    };
                }

                return new ResultadoRegistro
                {
                    Exito = false,
                    Mensaje = "Error al registrar el usuario",
                    PasswordTemporal = string.Empty,
                    UserName = string.Empty
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar usuario", ex);
            }
        }

        public bool EditarUsuario(entUsuario u)
        {
            try
            {
                return datUsuario.Instancia.EditarUsuario(u);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar usuario", ex);
            }
        }

        //public bool CambiarEstadoUsuario(int id, bool estado)
        //{
        //    try
        //    {
        //        return datUsuario.Instancia.CambiarEstadoUsuario(id, estado);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al cambiar el estado del usuario", ex);
        //    }
        //}

        #endregion

        #region Método para Autenticación
        public entUsuario Autenticar(string userName, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                    return null;

                // Encriptar la contraseña para comparar con la base de datos
                string passwordEncriptado = Recursos.EncriptarSHA256(password);

                // Llamar al método de datos para autenticar
                return datUsuario.Instancia.AutenticarUsuario(userName, passwordEncriptado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Autenticar: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}
