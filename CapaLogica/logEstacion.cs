using CapaDatos;
using CapaEntidad;

namespace CapaLogica
{
    public class logEstacion
    {
        #region Singleton
        private static readonly logEstacion _instancia = new logEstacion();
        public static logEstacion Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos
        public List<entEstacion> ListarEstaciones()
        {
            try
            {
                return datEstacion.Instancia.ListarEstaciones();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertarEstacion(entEstacion e)
        {
            try
            {
                return datEstacion.Instancia.InsertarEstacion(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EditarEstacion(entEstacion e)
        {
            try
            {
                return datEstacion.Instancia.EditarEstacion(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<entEstacion> ListarEstacionesActivas()
        {
            try
            {
                return datEstacion.Instancia.ListarEstacionesActivas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public bool EliminarEstacion(int id)
        //{
        //    try
        //    {
        //        return datEstacion.Instancia.EliminarEstacion(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }
}
