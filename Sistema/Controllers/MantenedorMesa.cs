using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

// Modelo simple para recibir las coordenadas
public class PosicionMesaModel
{
    public int MesaId { get; set; }
    public int PosicionX { get; set; }
    public int PosicionY { get; set; }
}

namespace Sistema.Controllers
{
    public class MantenedorMesa : Controller
    {
        public IActionResult ListarMesas()
        {
            return View();
        }
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

        [HttpGet]
        public JsonResult ListarMesasPorZona(int zonaId)
        {
            try
            {
                var lista = logMesa.Instancia.ListarMesasPorZona(zonaId);
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

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


        [HttpPost]
        public JsonResult ActualizarPosicion(int mesaId, decimal posicionX, decimal posicionY)
        {
            try
            {
                bool resultado = logMesa.Instancia.ActualizarPosicionMesa(mesaId, posicionX, posicionY);
                return Json(new { resultado = resultado, mensaje = "Posición actualizada" });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = "Error: " + ex.Message });
            }
        }

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
