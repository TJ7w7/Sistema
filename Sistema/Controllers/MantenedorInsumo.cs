using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MantenedorInsumo : Controller
    {
        public IActionResult ListarInsumos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerInsumos()
        {
            try
            {
                var lista = logInsumo.Instancia.ListarInsumosConEstaciones();

                var resultado = lista.Select(i => new
                {
                    i.InsumoId,
                    i.Nombre,
                    i.UnidadMedida,
                    i.Estado,
                    CantidadEstaciones = i.Estaciones.Count,
                    Estaciones = i.Estaciones.Select(e => new
                    {
                        e.InsumoEstacionId,
                        e.EstacionId,
                        e.NombreEstacion,
                        e.StockActual,
                        e.StockMinimo,
                        e.EsBajoStock
                    }).ToList(),
                    TieneBajoStock = i.Estaciones.Any(e => e.EsBajoStock)
                }).ToList();

                return Json(new { data = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<object>(), error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarInsumo([FromBody] entInsumo insumo)
        {
            try
            {
                if (insumo == null)
                    return Json(new { exito = false, mensaje = "Datos inválidos" });

                bool resultado;
                int nuevoId = 0;

                if (insumo.InsumoId == 0)
                {
                    // Insertar
                    nuevoId = logInsumo.Instancia.InsertarInsumo(insumo);
                    resultado = nuevoId > 0;
                }
                else
                {
                    // Editar
                    resultado = logInsumo.Instancia.EditarInsumo(insumo);
                }

                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Insumo guardado correctamente" : "Error al guardar insumo",
                    insumoId = nuevoId
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult ObtenerEstacionesPorInsumo(int insumoId)
        {
            try
            {
                var lista = logInsumo.Instancia.ListarEstacionesPorInsumo(insumoId);
                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AsignarEstacion([FromBody] entInsumoEstacion insumoEstacion)
        {
            try
            {
                bool resultado = logInsumo.Instancia.AsignarInsumoEstacion(insumoEstacion);
                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Stock asignado correctamente" : "Error al asignar stock"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ActualizarStock(int insumoEstacionId, decimal stockActual)
        {
            try
            {
                bool resultado = logInsumo.Instancia.ActualizarStock(insumoEstacionId, stockActual);
                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Stock actualizado" : "Error al actualizar stock"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarAsignacion(int insumoEstacionId)
        {
            try
            {
                bool resultado = logInsumo.Instancia.EliminarInsumoEstacion(insumoEstacionId);
                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Asignación eliminada" : "Error al eliminar"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult ObtenerEstacionesActivas()
        {
            try
            {
                var lista = logEstacion.Instancia.ListarEstacionesActivas()
                    .Select(e => new
                    {
                        e.EstacionId,
                        e.Nombre
                    }).ToList();
                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }
    }
}
