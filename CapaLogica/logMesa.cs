using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logMesa
    {
        #region Singleton
        private static readonly logMesa _instancia = new logMesa();
        public static logMesa Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        public List<entMesa> ListarMesas()
        {
            try
            {
                return datMesa.Instancia.ListarMesas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar mesas: " + ex.Message);
            }
        }

        public List<entMesa> ListarMesasPorZona(int zonaId)
        {
            try
            {
                return datMesa.Instancia.ListarMesasPorZona(zonaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar mesas por zona: " + ex.Message);
            }
        }

        public int InsertarMesa(entMesa m)
        {
            try
            {
                return datMesa.Instancia.InsertarMesa(m);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar mesa: " + ex.Message);
            }
        }

        public bool ActualizarMesa(entMesa m)
        {
            try
            {
                return datMesa.Instancia.ActualizarMesa(m);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar mesa: " + ex.Message);
            }
        }

        public bool ActualizarPosicionMesa(int mesaId, decimal posicionX, decimal posicionY)
        {
            try
            {
                return datMesa.Instancia.ActualizarPosicionMesa(mesaId, posicionX, posicionY);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar posición: " + ex.Message);
            }
        }

        public bool EliminarMesa(int mesaId)
        {
            try
            {
                return datMesa.Instancia.EliminarMesa(mesaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar mesa: " + ex.Message);
            }
        }

        #endregion
    }
}
