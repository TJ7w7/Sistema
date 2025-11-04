using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class CocinaController : Controller
    {
        public IActionResult Cocina()
        {
            // Obtener el EstacionId del usuario autenticado
            var estacionIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EstacionId")?.Value;

            if (string.IsNullOrEmpty(estacionIdClaim))
            {
                ViewBag.Error = "No tiene una estación asignada. Contacte al administrador.";
                return View("Error");
            }

            int estacionId = int.Parse(estacionIdClaim);

            // Obtener información de la estación
            var estacion = logEstacion.Instancia.ListarEstacionesActivas()
                .FirstOrDefault(e => e.EstacionId == estacionId);

            if (estacion == null)
            {
                ViewBag.Error = "Estación no encontrada o inactiva.";
                return View("Error");
            }

            ViewBag.EstacionId = estacionId;
            ViewBag.EstacionNombre = estacion.Nombre;

            var nombreCompleto = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value ?? "Cocinero";
            ViewBag.NombreUsuario = nombreCompleto;

            return View();
        }

        // Obtener detalles pendientes/en preparación de la estación
        [HttpGet]
        public JsonResult ObtenerDetallesEstacion(int estacionId, string estadoDetalle = null)
        {
            try
            {
                // Validar que el usuario tenga acceso a esta estación
                var estacionIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EstacionId")?.Value;

                if (string.IsNullOrEmpty(estacionIdClaim) || int.Parse(estacionIdClaim) != estacionId)
                {
                    // Solo admin puede ver todas las estaciones
                    if (!User.IsInRole("ADMINISTRADOR"))
                    {
                        return Json(new { success = false, mensaje = "No tiene acceso a esta estación" });
                    }
                }

                var detalles = logPedido.Instancia.ListarDetallesPorEstacion(estacionId, estadoDetalle);

                return Json(new { success = true, data = detalles });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Cambiar estado de un detalle
        [HttpPost]
        public JsonResult CambiarEstadoDetalle(int detalleId, string nuevoEstado)
        {
            try
            {
                bool resultado = logPedido.Instancia.ActualizarEstadoDetalle(detalleId, nuevoEstado);

                return Json(new
                {
                    success = resultado,
                    mensaje = resultado ? "Estado actualizado correctamente" : "Error al actualizar estado"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Marcar como "En Preparación"
        [HttpPost]
        public JsonResult IniciarPreparacion(int detalleId)
        {
            try
            {
                bool resultado = logPedido.Instancia.ActualizarEstadoDetalle(detalleId, "En Preparación");

                return Json(new
                {
                    success = resultado,
                    mensaje = resultado ? "Preparación iniciada" : "Error al iniciar preparación"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Marcar como "Listo"
        [HttpPost]
        public JsonResult MarcarComoListo(int detalleId)
        {
            try
            {
                bool resultado = logPedido.Instancia.ActualizarEstadoDetalle(detalleId, "Listo");

                return Json(new
                {
                    success = resultado,
                    mensaje = resultado ? "Producto marcado como listo" : "Error al marcar como listo"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Obtener estadísticas de la cocina
        [HttpGet]
        public JsonResult ObtenerEstadisticas(int estacionId)
        {
            try
            {
                var detalles = logPedido.Instancia.ListarDetallesPorEstacion(estacionId, null);

                var estadisticas = new
                {
                    pendientes = detalles.Count(d => d.EstadoDetalle == "Pendiente"),
                    enPreparacion = detalles.Count(d => d.EstadoDetalle == "En Preparación"),
                    listos = detalles.Count(d => d.EstadoDetalle == "Listo"),
                    total = detalles.Count
                };

                return Json(new { success = true, data = estadisticas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }
    }
}
