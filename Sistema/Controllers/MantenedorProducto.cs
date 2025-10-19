using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MantenedorProducto : Controller
    {
        public IActionResult ListarProductos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerProductos()
        {
            try
            {
                var lista = logProducto.Instancia.ListarProductos();

                // Formatear datos para el DataTable
                var resultado = lista.Select(p => new
                {
                    p.ProductoId,
                    p.Nombre,
                    p.Descripcion,
                    p.CategoriaId,
                    p.NombreCategoria,
                    p.EstacionId,
                    p.NombreEstacion,
                    p.Imagen,
                    p.Estado,
                    Variantes = p.Variantes.Select(v => new
                    {
                        v.VarianteId,
                        v.Tamaño,
                        v.Precio
                    }).ToList(),
                    // Precio mínimo para mostrar en la tabla
                    PrecioMinimo = p.Variantes.Any() ? p.Variantes.Min(v => v.Precio) : 0,
                    // Precio máximo para mostrar en la tabla
                    PrecioMaximo = p.Variantes.Any() ? p.Variantes.Max(v => v.Precio) : 0
                }).ToList();

                return Json(new { data = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<object>(), error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SubirImagen(IFormFile imagen)
        {
            try
            {
                if (imagen == null || imagen.Length == 0)
                    return Json(new { exito = false, mensaje = "No se seleccionó ninguna imagen" });

                // Validar tipo de archivo
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var extension = Path.GetExtension(imagen.FileName).ToLower();

                if (!extensionesPermitidas.Contains(extension))
                    return Json(new { exito = false, mensaje = "Solo se permiten imágenes JPG, PNG, GIF o BMP" });

                // Validar tamaño (max 5MB)
                if (imagen.Length > 5 * 1024 * 1024)
                    return Json(new { exito = false, mensaje = "La imagen no debe superar 5MB" });

                // Convertir a bytes
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    imagen.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                // Guardar usando el servicio
                string nombreArchivo = ServicioImagen.Instancia.GuardarImagen(bytes, imagen.FileName);

                return Json(new
                {
                    exito = true,
                    mensaje = "Imagen subida correctamente",
                    nombreArchivo = nombreArchivo
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarImagen(string nombreArchivo)
        {
            try
            {
                bool resultado = ServicioImagen.Instancia.EliminarImagen(nombreArchivo);
                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Imagen eliminada" : "No se pudo eliminar la imagen"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarProducto([FromBody] ProductoRequest request)
        {
            try
            {
                if (request == null)
                    return Json(new { exito = false, mensaje = "Datos inválidos" });

                // Validar que tenga al menos una variante
                if (request.Variantes == null || request.Variantes.Count == 0)
                    return Json(new { exito = false, mensaje = "Debe agregar al menos un precio/tamaño" });

                var producto = new entProducto
                {
                    ProductoId = request.ProductoId,
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    CategoriaId = request.CategoriaId,
                    EstacionId = request.EstacionId,
                    Imagen = request.Imagen,
                    Estado = request.Estado,
                    Variantes = request.Variantes.Select(v => new entProductoVariante
                    {
                        Tamaño = v.Tamaño,
                        Precio = v.Precio
                    }).ToList()
                };

                bool resultado;

                if (producto.ProductoId == 0)
                {
                    // Insertar nuevo producto con variantes
                    resultado = logProducto.Instancia.InsertarProductoConVariantes(producto);
                }
                else
                {
                    // Editar producto con variantes
                    resultado = logProducto.Instancia.EditarProductoConVariantes(producto);
                }

                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Producto guardado correctamente" : "Error al guardar producto"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }


        // En MantenedorProducto Controller

        [HttpGet]
        public JsonResult ObtenerCategorias()
        {
            try
            {
                var lista = logCategoria.Instancia.ListarCategoriasActivas()
                    .Select(c => new
                    {
                        c.CategoriaId,
                        c.Nombre
                    }).ToList();
                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

        [HttpGet]
        public JsonResult ObtenerEstaciones()
        {
            try
            {
                var lista = logEstacion.Instancia.ListarEstacionesActivas()
                    .Select(e => new
                    {
                        e.EstacionId,
                        e.Nombre
                    }).ToList();
                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

        // Agregar al MantenedorProducto Controller
        [HttpGet]
        public JsonResult ObtenerReceta(int productoId)
        {
            try
            {
                var receta = logDetalleReceta.Instancia.ListarRecetaPorProducto(productoId);
                return Json(receta);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarReceta([FromBody] RecetaRequest request)
        {
            try
            {
                var receta = request.Ingredientes.Select(i => new entDetalleReceta
                {
                    ProductoId = request.ProductoId,
                    InsumoId = i.InsumoId,
                    Cantidad = i.Cantidad
                }).ToList();

                bool resultado = logDetalleReceta.Instancia.GuardarReceta(request.ProductoId, receta);

                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Receta guardada correctamente" : "Error al guardar receta"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarIngrediente(int productoId, int insumoId)
        {
            try
            {
                bool resultado = logDetalleReceta.Instancia.EliminarDetalleReceta(productoId, insumoId);
                return Json(new
                {
                    exito = resultado,
                    mensaje = resultado ? "Ingrediente eliminado" : "Error al eliminar"
                });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult ObtenerInsumosActivos()
        {
            try
            {
                var lista = logInsumo.Instancia.ListarInsumos()
                    .Where(i => i.Estado)
                    .Select(i => new
                    {
                        i.InsumoId,
                        i.Nombre,
                        i.UnidadMedida
                    }).ToList();
                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

        // Clase auxiliar
        public class RecetaRequest
        {
            public int ProductoId { get; set; }
            public List<IngredienteRequest> Ingredientes { get; set; }
        }

        public class IngredienteRequest
        {
            public int InsumoId { get; set; }
            public decimal Cantidad { get; set; }
        }

        //Mover a a capa entidad
        public class ProductoRequest
        {
            public int ProductoId { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public int CategoriaId { get; set; }
            public int? EstacionId { get; set; }
            public string Imagen { get; set; }
            public bool Estado { get; set; }
            public List<VarianteRequest> Variantes { get; set; }
        }

        public class VarianteRequest
        {
            public string Tamaño { get; set; }
            public decimal Precio { get; set; }
        }
    }
}
