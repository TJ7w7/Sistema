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
        // Listar mesas activas
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

        // Listar todas las mesas (incluye eliminadas)
        public List<entMesa> ListarMesasTodo()
        {
            try
            {
                return datMesa.Instancia.ListarMesasTodo();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar todas las mesas: " + ex.Message);
            }
        }

        // Insertar mesa
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

        // Actualizar mesa
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

        // Eliminar mesa (lógica)
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
