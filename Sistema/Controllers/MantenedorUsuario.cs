using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    [Authorize(Roles = "Administrador,Admin")]
    public class MantenedorUsuario : Controller
    {
        public ActionResult ListarUsuarios()
        {
            return View();
        }

        // ✅ Obtener lista (para DataTables)
        [HttpGet]
        public JsonResult ObtenerUsuarios()
        {
            try
            {
                var lista = logUsuario.Instancia.ListarUsuarios();
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<entUsuario>(), error = ex.Message });
            }
        }

        // ✅ Guardar (insertar o editar usuario)
        [HttpPost]
        public JsonResult GuardarUsuario([FromBody] entUsuario u)
        {
            object resultado;
            string mensaje = string.Empty;
            string passwordTemporal = string.Empty;
            string userName = string.Empty;

            if (u == null)
            {
                return Json(new { resultado = 0, mensaje = "Datos inválidos" });
            }

            try
            {
                if (u.UsuarioId == 0)
                {
                    // Registrar nuevo usuario
                    var respuesta = logUsuario.Instancia.RegistrarUsuario(u);
                    resultado = respuesta.Exito ? 1 : 0;
                    mensaje = respuesta.Mensaje;
                    passwordTemporal = respuesta.PasswordTemporal;
                    userName = respuesta.UserName; // ⭐ Obtener userName generado
                }
                else
                {
                    // Editar usuario existente
                    bool actualizado = logUsuario.Instancia.EditarUsuario(u);
                    resultado = actualizado ? 1 : 0;
                    mensaje = actualizado ? "Usuario actualizado correctamente" : "Error al actualizar el usuario";
                }

                return Json(new { resultado, mensaje, passwordTemporal, userName });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = 0, mensaje = "Error: " + ex.Message });
            }
        }

        // ✅ Obtener un usuario por ID (para editar)
        [HttpGet]
        public JsonResult ObtenerUsuarioPorId(int id)
        {
            try
            {
                var usuario = logUsuario.Instancia.ObtenerUsuarioPorId(id);
                if (usuario == null)
                    return Json(new { resultado = 0, mensaje = "Usuario no encontrado" });

                return Json(new { resultado = 1, data = usuario });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = 0, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult ObtenerEstacionesActivas()
        {
            try
            {
                var lista = logEstacion.Instancia.ListarEstacionesActivas()
                    .Where(e => e.Estado == true)
                    .Select(e => new
                    {
                        e.EstacionId,
                        e.Nombre
                    }).ToList();

                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }


        // ✅ Cambiar estado (activar / desactivar)
        //[HttpPost]
        //public JsonResult CambiarEstadoUsuario(int idUsuario, bool nuevoEstado)
        //{
        //    try
        //    {
        //        bool cambiado = logUsuario.Instancia.CambiarEstadoUsuario(idUsuario, nuevoEstado);

        //        string mensaje = cambiado
        //            ? (nuevoEstado ? "Usuario activado correctamente" : "Usuario desactivado correctamente")
        //            : "No se pudo cambiar el estado del usuario";

        //        return Json(new { resultado = cambiado ? 1 : 0, mensaje });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { resultado = 0, mensaje = "Error: " + ex.Message });
        //    }
        //}
    }
}
