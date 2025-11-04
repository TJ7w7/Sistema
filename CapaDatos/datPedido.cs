using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datPedido
    {
        #region Singleton
        private static readonly datPedido _instancia = new datPedido();
        public static datPedido Instancia
        {
            get { return _instancia; }
        }
        #endregion

        // Insertar Pedido con Detalle (transaccional)
        public int InsertarPedidoConDetalle(entPedido pedido)
        {
            SqlCommand cmd = null;
            SqlConnection cn = null;
            int nuevoPedidoId = 0;

            try
            {
                cn = Conexion.Instancia.Conectar();
                cn.Open();

                // Iniciar transacción
                SqlTransaction transaction = cn.BeginTransaction();

                try
                {
                    // 1. Insertar el pedido
                    cmd = new SqlCommand("sp_InsertarPedidoConDetalle", cn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MesaId", pedido.MesaId);
                    cmd.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                    cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                    cmd.Parameters.AddWithValue("@PrecioTotal", pedido.PrecioTotal);

                    SqlParameter paramNuevoPedidoId = new SqlParameter("@NuevoPedidoId", SqlDbType.Int);
                    paramNuevoPedidoId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramNuevoPedidoId);

                    cmd.ExecuteNonQuery();
                    nuevoPedidoId = Convert.ToInt32(paramNuevoPedidoId.Value);

                    // 2. Insertar detalles del pedido
                    foreach (var detalle in pedido.Detalles)
                    {
                        cmd = new SqlCommand("sp_InsertarDetallePedido", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PedidoId", nuevoPedidoId);
                        cmd.Parameters.AddWithValue("@VarianteId", detalle.VarianteId);
                        cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@SubTotal", detalle.SubTotal);
                        cmd.ExecuteNonQuery();
                    }

                    // Confirmar transacción
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Revertir transacción en caso de error
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                    cn.Close();
            }

            return nuevoPedidoId;
        }

        // Listar Pedidos Activos
        public List<entPedido> ListarPedidosActivos()
        {
            SqlCommand cmd = null;
            List<entPedido> lista = new List<entPedido>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarPedidosActivos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entPedido p = new entPedido
                    {
                        PedidoId = Convert.ToInt32(dr["PedidoId"]),
                        MesaId = Convert.ToInt32(dr["MesaId"]),
                        NroMesa = Convert.ToInt32(dr["NroMesa"]),
                        UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                        NombreMozo = dr["NombreMozo"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        PrecioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                        Estado = Convert.ToBoolean(dr["Estado"]),
                        CantidadItems = Convert.ToInt32(dr["CantidadItems"])
                    };
                    lista.Add(p);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return lista;
        }

        // Obtener Detalle de un Pedido
        public List<entDetallePedido> ObtenerDetallePedido(int pedidoId)
        {
            SqlCommand cmd = null;
            List<entDetallePedido> lista = new List<entDetallePedido>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ObtenerDetallePedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entDetallePedido d = new entDetallePedido
                    {
                        DetalleId = Convert.ToInt32(dr["DetalleId"]),
                        PedidoId = Convert.ToInt32(dr["PedidoId"]),
                        VarianteId = Convert.ToInt32(dr["VarianteId"]),
                        ProductoId = Convert.ToInt32(dr["ProductoId"]),
                        NombreProducto = dr["NombreProducto"].ToString(),
                        Tamaño = dr["Tamaño"].ToString(),
                        EstacionId = dr["EstacionId"] != DBNull.Value ? Convert.ToInt32(dr["EstacionId"]) : (int?)null,
                        NombreEstacion = dr["NombreEstacion"].ToString(),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                        SubTotal = Convert.ToDecimal(dr["SubTotal"]),
                        EstadoDetalle = dr["EstadoDetalle"].ToString()
                    };
                    lista.Add(d);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return lista;
        }

        // Actualizar Estado de Pedido
        public bool ActualizarEstadoPedido(int pedidoId, bool estado)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarEstadoPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }

        // Actualizar Estado de un Detalle específico
        public bool ActualizarEstadoDetalle(int detalleId, string estadoDetalle)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarEstadoDetalle", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DetalleId", detalleId);
                cmd.Parameters.AddWithValue("@EstadoDetalle", estadoDetalle);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }

        // Actualizar Estado de todos los detalles de un pedido
        public bool ActualizarEstadosDetallesPorPedido(int pedidoId, string estadoDetalle)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarEstadosDetallesPorPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                cmd.Parameters.AddWithValue("@EstadoDetalle", estadoDetalle);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }

        // Listar Detalles por Estación (para la cocina)
        public List<entDetallePedido> ListarDetallesPorEstacion(int estacionId, string estadoDetalle = null)
        {
            SqlCommand cmd = null;
            List<entDetallePedido> lista = new List<entDetallePedido>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarDetallesPorEstacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstacionId", estacionId);
                cmd.Parameters.AddWithValue("@EstadoDetalle", (object)estadoDetalle ?? DBNull.Value);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entDetallePedido d = new entDetallePedido
                    {
                        DetalleId = Convert.ToInt32(dr["DetalleId"]),
                        PedidoId = Convert.ToInt32(dr["PedidoId"]),
                        VarianteId = Convert.ToInt32(dr["VarianteId"]),
                        ProductoId = Convert.ToInt32(dr["ProductoId"]),
                        NombreProducto = dr["NombreProducto"].ToString(),
                        Tamaño = dr["Tamaño"].ToString(),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                        SubTotal = Convert.ToDecimal(dr["SubTotal"]),
                        EstadoDetalle = dr["EstadoDetalle"].ToString()
                    };
                    lista.Add(d);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return lista;
        }
        // Obtener pedido activo por mesa
        public entPedido ObtenerPedidoPorMesa(int mesaId)
        {
            SqlCommand cmd = null;
            entPedido pedido = null;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ObtenerPedidoPorMesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MesaId", mesaId);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    pedido = new entPedido
                    {
                        PedidoId = Convert.ToInt32(dr["PedidoId"]),
                        MesaId = Convert.ToInt32(dr["MesaId"]),
                        NroMesa = Convert.ToInt32(dr["NroMesa"]),
                        UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                        NombreMozo = dr["NombreMozo"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        PrecioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return pedido;
        }

        // Eliminar un detalle
        public bool EliminarDetalle(int detalleId)
        {
            SqlCommand cmd = null;
            bool elimina = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EliminarDetalle", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DetalleId", detalleId);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) elimina = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return elimina;
        }

        // Agregar detalle a pedido existente
        public bool AgregarDetalle(int pedidoId, entDetallePedido detalle)
        {
            SqlCommand cmd = null;
            bool agrega = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_AgregarDetalle", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                cmd.Parameters.AddWithValue("@VarianteId", detalle.VarianteId);
                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                cmd.Parameters.AddWithValue("@SubTotal", detalle.SubTotal);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) agrega = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return agrega;
        }

        // Actualizar cantidad de un detalle
        public bool ActualizarCantidadDetalle(int detalleId, int nuevaCantidad)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarCantidadDetalle", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DetalleId", detalleId);
                cmd.Parameters.AddWithValue("@NuevaCantidad", nuevaCantidad);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }

        // Actualizar total del pedido
        public bool ActualizarTotalPedido(int pedidoId, decimal nuevoTotal)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarTotalPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                cmd.Parameters.AddWithValue("@NuevoTotal", nuevoTotal);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }
    }
}
