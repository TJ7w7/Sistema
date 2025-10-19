using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MantenedorZona : Controller
    {
        public IActionResult ListarZonasMesas()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ObtenerZonas()
        {
            try
            {
                var lista = logZona.Instancia.ListarZonas();
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarZona([FromBody] entZona zona)
        {
            try
            {
                if (zona.ZonaId == 0)
                {
                    int nuevoId = logZona.Instancia.InsertarZona(zona);
                    return Json(new { resultado = true, mensaje = "Zona registrada correctamente", id = nuevoId });
                }
                else
                {
                    bool editado = logZona.Instancia.EditarZona(zona);
                    return Json(new { resultado = editado, mensaje = "Zona actualizada correctamente" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = "Error: " + ex.Message });
            }
        }
    }
}
