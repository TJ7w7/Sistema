using CapaLogica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Controllers
{
    public class MozoController : Controller
    {
        [Authorize(Roles = "Mozo")]
        public IActionResult Mozo()
        {
            //ViewBag.NombreMesero = "Nombre del Mesero"; // Reemplazar con dato real
            //return View();

            // Obtener el nombre completo del claim
            var nombreCompleto = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

            // Si no existe el claim FullName, construirlo desde Name
            if (string.IsNullOrEmpty(nombreCompleto))
            {
                nombreCompleto = User.Identity.Name ?? "Mesero";
            }

            ViewBag.NombreMesero = nombreCompleto;

            // También puedes pasar otros datos útiles
            ViewBag.UsuarioId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            ViewBag.UserName = User.Identity.Name;

            return View();
        }
        // Obtener mesas activas para el mozo
        [HttpGet]
        public JsonResult ObtenerMesasActivas()
        {
            try
            {
                var lista = logMesa.Instancia.ListarMesas();
                return Json(new { success = true, data = lista });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }
    }
}
