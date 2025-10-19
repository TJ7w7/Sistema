using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logZona
    {
        #region Singleton
        private static readonly logZona _instancia = new logZona();
        public static logZona Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        public List<entZona> ListarZonas()
        {
            try
            {
                return datZona.Instancia.ListarZonas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las zonas: " + ex.Message);
            }
        }

        public int InsertarZona(entZona z)
        {
            try
            {
                return datZona.Instancia.InsertarZona(z);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar zona: " + ex.Message);
            }
        }

        public bool EditarZona(entZona z)
        {
            try
            {
                return datZona.Instancia.EditarZona(z);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar zona: " + ex.Message);
            }
        }

        #endregion
    }
}
