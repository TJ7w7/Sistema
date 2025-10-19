using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logProducto
    {
        #region Singleton
        private static readonly logProducto _instancia = new logProducto();
        public static logProducto Instancia
        {
            get { return _instancia; }
        }
        #endregion

        public List<entProducto> ListarProductos()
        {
            try
            {
                return datProducto.Instancia.ListarProductos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar productos", ex);
            }
        }

        public List<entProducto> ListarProductosActivos()
        {
            try
            {
                return datProducto.Instancia.ListarProductosActivos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar productos activos", ex);
            }
        }

        // CapaLogica/logProducto.cs
        public bool InsertarProductoConVariantes(entProducto producto)
        {
            try
            {
                // Insertar producto y obtener su ID
                int nuevoId = datProducto.Instancia.InsertarProducto(producto);

                if (nuevoId > 0 && producto.Variantes != null && producto.Variantes.Count > 0)
                {
                    // Insertar variantes
                    foreach (var variante in producto.Variantes)
                    {
                        variante.ProductoId = nuevoId;
                        datProductoVariante.Instancia.InsertarVariante(variante);
                    }
                    return true;
                }

                return nuevoId > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar producto con variantes", ex);
            }
        }

        public bool EditarProductoConVariantes(entProducto producto)
        {
            try
            {
                // Editar producto
                bool resultado = datProducto.Instancia.EditarProducto(producto);

                if (resultado)
                {
                    // Eliminar variantes anteriores
                    datProductoVariante.Instancia.EliminarVariantesPorProducto(producto.ProductoId);

                    // Insertar nuevas variantes
                    if (producto.Variantes != null && producto.Variantes.Count > 0)
                    {
                        foreach (var variante in producto.Variantes)
                        {
                            variante.ProductoId = producto.ProductoId;
                            datProductoVariante.Instancia.InsertarVariante(variante);
                        }
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar producto con variantes", ex);
            }
        }
    }
}
