using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MantenedorMesa : Controller
    {
        // Vista principal
        public IActionResult ListarMesas()
        {
            return View();
        }

        // Obtener todas las mesas activas
        [HttpGet]
        public JsonResult ObtenerMesas()
        {
            try
            {
                var lista = logMesa.Instancia.ListarMesas();
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Obtener todas las mesas (incluye eliminadas)
        [HttpGet]
        public JsonResult ObtenerMesasTodo()
        {
            try
            {
                var lista = logMesa.Instancia.ListarMesasTodo();
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Guardar mesa (insertar o actualizar)
        [HttpPost]
        public JsonResult GuardarMesa([FromBody] entMesa mesa)
        {
            try
            {
                if (mesa.MesaId == 0)
                {
                    int nuevoId = logMesa.Instancia.InsertarMesa(mesa);
                    return Json(new { resultado = true, mensaje = "Mesa registrada correctamente", id = nuevoId });
                }
                else
                {
                    bool actualizado = logMesa.Instancia.ActualizarMesa(mesa);
                    return Json(new { resultado = actualizado, mensaje = "Mesa actualizada correctamente" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Eliminar mesa (lógica)
        [HttpPost]
        public JsonResult EliminarMesa(int mesaId)
        {
            try
            {
                bool resultado = logMesa.Instancia.EliminarMesa(mesaId);
                return Json(new { resultado = resultado, mensaje = "Mesa eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = "Error: " + ex.Message });
            }
        }
    }
}
