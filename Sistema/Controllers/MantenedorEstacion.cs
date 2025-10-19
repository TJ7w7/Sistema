using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MantenedorEstacion : Controller
    {
        public IActionResult ListarEstaciones()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerEstaciones()
        {
            var lista = logEstacion.Instancia.ListarEstaciones();
            return Json(new { data = lista });
        }

        [HttpPost]
        public JsonResult GuardarEstacion([FromBody] entEstacion e)
        {
            object resultado;
            string mensaje = string.Empty;

            if (e == null)
            {
                return Json(new { resultado = 0, mensaje = "Datos inválidos" });
            }
            if (e.EstacionId == 0)
            {
                resultado = logEstacion.Instancia.InsertarEstacion(e) ? 1 : 0;
                mensaje = (int)resultado == 1 ? "Estación registrada correctamente" : "Error al registrar la estación";
            }
            else
            {
                resultado = logEstacion.Instancia.EditarEstacion(e);
                mensaje = (bool)resultado ? "Estación actualizada correctamente" : "Error al actualizar la estación";
            }

            return Json(new { resultado, mensaje });
        }

        //[HttpPost]
        //public JsonResult GuardarEstacion([FromBody] entEstacion e)
        //{
        //    try
        //    {
        //        // ✅ Validar que el objeto no sea null
        //        if (e == null)
        //        {
        //            return Json(new
        //            {
        //                resultado = 0,
        //                mensaje = "Error: No se recibieron datos de la estación"
        //            });
        //        }

        //        // ✅ Validar propiedades requeridas
        //        if (string.IsNullOrEmpty(e.Nombre)) // Ajusta según tus propiedades
        //        {
        //            return Json(new
        //            {
        //                resultado = 0,
        //                mensaje = "Error: El nombre de la estación es requerido"
        //            });
        //        }

        //        object resultado;
        //        string mensaje = string.Empty;

        //        if (e.EstacionId == 0)
        //        {
        //            resultado = logEstacion.Instancia.InsertarEstacion(e) ? 1 : 0;
        //            mensaje = (int)resultado == 1 ? "Estación registrada correctamente" : "Error al registrar la estación";
        //        }
        //        else
        //        {
        //            resultado = logEstacion.Instancia.EditarEstacion(e);
        //            mensaje = (bool)resultado ? "Estación actualizada correctamente" : "Error al actualizar la estación";
        //        }

        //        return Json(new { resultado, mensaje });
        //    }
        //    catch (Exception ex)
        //    {
        //        // ✅ Manejar cualquier excepción
        //        return Json(new
        //        {
        //            resultado = 0,
        //            mensaje = $"Error interno: {ex.Message}"
        //        });
        //    }
        //}

        //[HttpPost]
        //public JsonResult EliminarEstacion(int id)
        //{
        //    var resultado = logEstacion.Instancia.EliminarEstacion(id);
        //    string mensaje = resultado ? "Estación eliminada correctamente" : "Error al eliminar la estación";

        //    return Json(new { resultado, mensaje });
        //}
    }
}
