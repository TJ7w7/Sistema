using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCategoria
    {
        #region Singleton
        private static readonly logCategoria _instancia = new logCategoria();
        public static logCategoria Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // 🔹 Listar todas las categorías
        public List<entCategoria> ListarCategorias()
        {
            try
            {
                return datCategoria.Instancia.ListarCategorias();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 🔹 Insertar una nueva categoría
        public bool InsertarCategoria(entCategoria c)
        {
            try
            {
                return datCategoria.Instancia.InsertarCategoria(c);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 🔹 Editar una categoría existente
        public bool EditarCategoria(entCategoria c)
        {
            try
            {
                return datCategoria.Instancia.EditarCategoria(c);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 🔹 Eliminar una categoría (por ID)
        //public bool EliminarCategoria(int idCategoria)
        //{
        //    try
        //    {
        //        return datCategoria.Instancia.EliminarCategoria(idCategoria);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // 🔹 Listar solo las categorías activas
        public List<entCategoria> ListarCategoriasActivas()
        {
            try
            {
                return datCategoria.Instancia.ListarCategoriasActivas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
