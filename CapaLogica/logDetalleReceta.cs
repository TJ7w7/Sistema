using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logDetalleReceta
    {
        #region Singleton
        private static readonly logDetalleReceta _instancia = new logDetalleReceta();
        public static logDetalleReceta Instancia
        {
            get { return _instancia; }
        }
        #endregion

        public List<entDetalleReceta> ListarRecetaPorProducto(int productoId)
        {
            try
            {
                return datDetalleReceta.Instancia.ListarRecetaPorProducto(productoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar receta", ex);
            }
        }

        public bool GuardarReceta(int productoId, List<entDetalleReceta> receta)
        {
            try
            {
                // Eliminar receta anterior
                datDetalleReceta.Instancia.EliminarRecetaCompleta(productoId);

                // Insertar nueva receta
                if (receta != null && receta.Count > 0)
                {
                    foreach (var detalle in receta)
                    {
                        detalle.ProductoId = productoId;
                        datDetalleReceta.Instancia.InsertarDetalleReceta(detalle);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar receta", ex);
            }
        }

        public bool InsertarDetalleReceta(entDetalleReceta detalle)
        {
            try
            {
                return datDetalleReceta.Instancia.InsertarDetalleReceta(detalle);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar ingrediente", ex);
            }
        }

        public bool EliminarDetalleReceta(int productoId, int insumoId)
        {
            try
            {
                return datDetalleReceta.Instancia.EliminarDetalleReceta(productoId, insumoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar ingrediente", ex);
            }
        }
    }
}
