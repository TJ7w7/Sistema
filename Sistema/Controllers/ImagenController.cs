using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class ImagenController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Producto(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                    return NotFound();

                string rutaCompleta = ServicioImagen.Instancia.ObtenerRutaCompleta(nombre);

                if (!System.IO.File.Exists(rutaCompleta))
                    return NotFound();

                var imagen = System.IO.File.OpenRead(rutaCompleta);
                string contentType = ObtenerContentType(nombre);

                return File(imagen, contentType);
            }
            catch
            {
                return NotFound();
            }
        }

        private string ObtenerContentType(string nombreArchivo)
        {
            string extension = Path.GetExtension(nombreArchivo).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };
        }
    }
}
