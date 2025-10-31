using CapaDatos;
using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    [Authorize(Roles = "MOZO,Mozo")]
    public class PedidoController : Controller
    {
        // Vista para crear pedido
        public IActionResult CrearPedido(int mesaId, int nroMesa)
        {
            if (mesaId <= 0 || nroMesa <= 0)
            {
                return RedirectToAction("Index", "Mozo");
            }

            ViewBag.MesaId = mesaId;
            ViewBag.NroMesa = nroMesa;

            // Obtener datos del mozo
            var nombreCompleto = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value ?? "Mesero";
            ViewBag.NombreMesero = nombreCompleto;

            return View();
        }

        // Obtener productos activos con variantes y categorías
        [HttpGet]
        public JsonResult ObtenerProductosActivos()
        {
            try
            {
                var productos = logProducto.Instancia.ListarProductosActivos();
                return Json(new { success = true, data = productos });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Guardar pedido
        [HttpPost]
        public JsonResult GuardarPedido([FromBody] entPedido pedido)
        {
            try
            {
                // Obtener el ID del usuario actual
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Json(new { success = false, mensaje = "Usuario no autenticado" });
                }

                pedido.UsuarioId = int.Parse(userIdClaim);

                // Insertar pedido
                int nuevoPedidoId = logPedido.Instancia.InsertarPedidoConDetalle(pedido);

                return Json(new
                {
                    success = true,
                    mensaje = "Pedido registrado correctamente",
                    pedidoId = nuevoPedidoId
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Listar pedidos activos
        [HttpGet]
        public JsonResult ListarPedidosActivos()
        {
            try
            {
                var pedidos = logPedido.Instancia.ListarPedidosActivos();
                return Json(new { success = true, data = pedidos });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Obtener detalle de un pedido
        [HttpGet]
        public JsonResult ObtenerDetallePedido(int pedidoId)
        {
            try
            {
                var detalle = logPedido.Instancia.ObtenerDetallePedido(pedidoId);
                return Json(new { success = true, data = detalle });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Finalizar pedido
        [HttpPost]
        public JsonResult FinalizarPedido(int pedidoId)
        {
            try
            {
                bool resultado = logPedido.Instancia.FinalizarPedido(pedidoId);
                return Json(new
                {
                    success = resultado,
                    mensaje = resultado ? "Pedido finalizado correctamente" : "Error al finalizar pedido"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Actualizar estado de un detalle específico
        [HttpPost]
        public JsonResult ActualizarEstadoDetalle(int detalleId, string estadoDetalle)
        {
            try
            {
                bool resultado = logPedido.Instancia.ActualizarEstadoDetalle(detalleId, estadoDetalle);
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

        // Actualizar estado de todos los detalles de un pedido
        [HttpPost]
        public JsonResult ActualizarEstadosDetallesPorPedido(int pedidoId, string estadoDetalle)
        {
            try
            {
                bool resultado = logPedido.Instancia.ActualizarEstadosDetallesPorPedido(pedidoId, estadoDetalle);
                return Json(new
                {
                    success = resultado,
                    mensaje = resultado ? "Estados actualizados correctamente" : "Error al actualizar estados"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        // Listar detalles por estación (para la cocina)
        [HttpGet]
        public JsonResult ListarDetallesPorEstacion(int estacionId, string estadoDetalle = null)
        {
            try
            {
                var detalles = logPedido.Instancia.ListarDetallesPorEstacion(estacionId, estadoDetalle);
                return Json(new { success = true, data = detalles });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

    }
}
