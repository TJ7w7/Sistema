using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logInsumo
    {
        #region Singleton
        private static readonly logInsumo _instancia = new logInsumo();
        public static logInsumo Instancia
        {
            get { return _instancia; }
        }
        #endregion

        public List<entInsumo> ListarInsumos()
        {
            try
            {
                return datInsumo.Instancia.ListarInsumos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar insumos", ex);
            }
        }

        public List<entInsumo> ListarInsumosConEstaciones()
        {
            try
            {
                return datInsumoEstacion.Instancia.ListarInsumosConEstaciones();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar insumos con estaciones", ex);
            }
        }

        public int InsertarInsumo(entInsumo insumo)
        {
            try
            {
                return datInsumo.Instancia.InsertarInsumo(insumo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar insumo", ex);
            }
        }

        public bool EditarInsumo(entInsumo insumo)
        {
            try
            {
                return datInsumo.Instancia.EditarInsumo(insumo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar insumo", ex);
            }
        }

        public List<entInsumoEstacion> ListarEstacionesPorInsumo(int insumoId)
        {
            try
            {
                return datInsumoEstacion.Instancia.ListarEstacionesPorInsumo(insumoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar estaciones del insumo", ex);
            }
        }

        public bool AsignarInsumoEstacion(entInsumoEstacion insumoEstacion)
        {
            try
            {
                return datInsumoEstacion.Instancia.AsignarInsumoEstacion(insumoEstacion);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar insumo a estación", ex);
            }
        }

        public bool ActualizarStock(int insumoEstacionId, decimal stockActual)
        {
            try
            {
                return datInsumoEstacion.Instancia.ActualizarStock(insumoEstacionId, stockActual);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar stock", ex);
            }
        }

        public bool EliminarInsumoEstacion(int insumoEstacionId)
        {
            try
            {
                return datInsumoEstacion.Instancia.EliminarInsumoEstacion(insumoEstacionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar asignación", ex);
            }
        }
    }
}
