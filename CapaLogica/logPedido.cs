using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logPedido
    {
        #region Singleton
        private static readonly logPedido _instancia = new logPedido();
        public static logPedido Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // Insertar Pedido con Detalle
        public int InsertarPedidoConDetalle(entPedido pedido)
        {
            try
            {
                // Validaciones
                if (pedido.MesaId <= 0)
                    throw new Exception("Debe seleccionar una mesa");

                if (pedido.UsuarioId <= 0)
                    throw new Exception("Usuario no válido");

                if (pedido.Detalles == null || pedido.Detalles.Count == 0)
                    throw new Exception("Debe agregar al menos un producto al pedido");

                if (pedido.PrecioTotal <= 0)
                    throw new Exception("El precio total debe ser mayor a cero");

                // Establecer la fecha actual
                pedido.Fecha = DateTime.Now;

                return datPedido.Instancia.InsertarPedidoConDetalle(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar pedido: " + ex.Message);
            }
        }

        // Listar Pedidos Activos
        public List<entPedido> ListarPedidosActivos()
        {
            try
            {
                return datPedido.Instancia.ListarPedidosActivos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar pedidos: " + ex.Message);
            }
        }

        // Obtener Detalle de un Pedido
        public List<entDetallePedido> ObtenerDetallePedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                    throw new Exception("ID de pedido no válido");

                return datPedido.Instancia.ObtenerDetallePedido(pedidoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalle del pedido: " + ex.Message);
            }
        }

        // Finalizar Pedido (cambiar estado a false)
        public bool FinalizarPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                    throw new Exception("ID de pedido no válido");

                return datPedido.Instancia.ActualizarEstadoPedido(pedidoId, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al finalizar pedido: " + ex.Message);
            }
        }

        // Reactivar Pedido
        public bool ReactivarPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                    throw new Exception("ID de pedido no válido");

                return datPedido.Instancia.ActualizarEstadoPedido(pedidoId, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar pedido: " + ex.Message);
            }
        }

        // Actualizar Estado de un Detalle específico
        public bool ActualizarEstadoDetalle(int detalleId, string estadoDetalle)
        {
            try
            {
                if (detalleId <= 0)
                    throw new Exception("ID de detalle no válido");

                // Validar estado
                string[] estadosValidos = { "Pendiente", "En Preparación", "Listo", "Entregado" };
                if (!estadosValidos.Contains(estadoDetalle))
                    throw new Exception("Estado no válido");

                return datPedido.Instancia.ActualizarEstadoDetalle(detalleId, estadoDetalle);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estado del detalle: " + ex.Message);
            }
        }

        // Actualizar Estado de todos los detalles de un pedido
        public bool ActualizarEstadosDetallesPorPedido(int pedidoId, string estadoDetalle)
        {
            try
            {
                if (pedidoId <= 0)
                    throw new Exception("ID de pedido no válido");

                // Validar estado
                string[] estadosValidos = { "Pendiente", "En Preparación", "Listo", "Entregado" };
                if (!estadosValidos.Contains(estadoDetalle))
                    throw new Exception("Estado no válido");

                return datPedido.Instancia.ActualizarEstadosDetallesPorPedido(pedidoId, estadoDetalle);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estados de detalles: " + ex.Message);
            }
        }

        // Listar Detalles por Estación (para la cocina)
        public List<entDetallePedido> ListarDetallesPorEstacion(int estacionId, string estadoDetalle = null)
        {
            try
            {
                if (estacionId <= 0)
                    throw new Exception("ID de estación no válido");

                // Validar estado si se proporciona
                if (!string.IsNullOrEmpty(estadoDetalle))
                {
                    string[] estadosValidos = { "Pendiente", "En Preparación", "Listo", "Entregado" };
                    if (!estadosValidos.Contains(estadoDetalle))
                        throw new Exception("Estado no válido");
                }

                return datPedido.Instancia.ListarDetallesPorEstacion(estacionId, estadoDetalle);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar detalles por estación: " + ex.Message);
            }
        }

        // Verificar si todos los detalles de un pedido están entregados
        public bool TodosLosDetallesEntregados(int pedidoId)
        {
            try
            {
                var detalles = ObtenerDetallePedido(pedidoId);
                return detalles.All(d => d.EstadoDetalle == "Entregado");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar detalles: " + ex.Message);
            }
        }

        #endregion
    }
}
