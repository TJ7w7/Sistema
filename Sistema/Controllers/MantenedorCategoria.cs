using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MantenedorCategoria : Controller
    {
        public IActionResult ListarCategorias()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerCategorias()
        {
            try
            {
                var lista = logCategoria.Instancia.ListarCategorias();
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<entCategoria>(), error = "Error al listar categorías: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarCategoria([FromBody] entCategoria c)
        {
            object resultado;
            string mensaje = string.Empty;

            if (c == null)
            {
                return Json(new { resultado = 0, mensaje = "Datos inválidos" });
            }

            try
            {
                if (c.CategoriaId == 0)
                {
                    resultado = logCategoria.Instancia.InsertarCategoria(c) ? 1 : 0;
                    mensaje = (int)resultado == 1 ? "Categoría registrada correctamente" : "Error al registrar la categoría";
                }
                else
                {
                    resultado = logCategoria.Instancia.EditarCategoria(c);
                    mensaje = (bool)resultado ? "Categoría actualizada correctamente" : "Error al actualizar la categoría";
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = "Ocurrió un error: " + ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        //[HttpPost]
        //public JsonResult EliminarCategoria(int idCategoria)
        //{
        //    bool resultado = false;
        //    string mensaje = string.Empty;

        //    try
        //    {
        //        resultado = logCategoria.Instancia.EliminarCategoria(idCategoria);
        //        mensaje = resultado ? "Categoría eliminada correctamente" : "Error al eliminar la categoría";
        //    }
        //    catch (Exception ex)
        //    {
        //        mensaje = "Error al eliminar categoría: " + ex.Message;
        //    }

        //    return Json(new { resultado, mensaje });
        //}

        [HttpGet]
        public JsonResult ObtenerCategoriasActivas()
        {
            try
            {
                var lista = logCategoria.Instancia.ListarCategoriasActivas();
                return Json(new { data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<entCategoria>(), error = "Error al listar categorías activas: " + ex.Message });
            }
        }
    }
}
