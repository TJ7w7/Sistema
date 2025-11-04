using CapaLogica;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Sistema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Obtener el rol del usuario autenticado
                var rol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                // Redirigir según el rol
                return RedirectBasedOnRole(rol);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpia sesión (solo si la usas)
            //HttpContext.Session?.Clear();

            return RedirectToAction("Index", "Home");
        }

        // POST: Home/Index - Procesar Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string username, string password, bool rememberMe = false)
        {
            try
            {
                // Validación de campos vacíos
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Usuario y contraseña son obligatorios";
                    return View();
                }

                // Autenticar usuario
                var usuario = logUsuario.Instancia.Autenticar(username, password);

                if (usuario == null)
                {
                    ViewBag.Error = "Usuario o contraseña incorrectos";
                    return View();
                }

                if (!usuario.Estado)
                {
                    ViewBag.Error = "Su cuenta está inactiva. Contacte al administrador";
                    return View();
                }

                // Crear claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
                    new Claim("UserId", usuario.UsuarioId.ToString())
                };

                // Agregar claims de estación si existen
                if (usuario.EstacionId.HasValue)
                {
                    claims.Add(new Claim("EstacionId", usuario.EstacionId.Value.ToString()));

                    if (!string.IsNullOrEmpty(usuario.EstacionNombre))
                    {
                        claims.Add(new Claim("EstacionNombre", usuario.EstacionNombre));
                    }
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe
                        ? DateTimeOffset.UtcNow.AddDays(30)
                        : DateTimeOffset.UtcNow.AddHours(24),
                    AllowRefresh = true
                };

                // Iniciar sesión
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                //// Redirigir según el rol
                //return RedirectBasedOnRole(usuario.Rol);
                var resultado = RedirectBasedOnRole(usuario.Rol);
                return resultado;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en login: {ex.Message}");
                ViewBag.Error = "Error al iniciar sesión. Por favor, intente nuevamente.";
                return View();
            }
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            var userName = User.Identity.Name;
            var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            var rol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var estacionNombre = User.Claims.FirstOrDefault(c => c.Type == "EstacionNombre")?.Value;

            ViewBag.UserName = userName;
            ViewBag.FullName = fullName ?? userName;
            ViewBag.Rol = rol;
            ViewBag.EstacionNombre = estacionNombre ?? "Sin estación asignada";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // Método privado para redirigir según rol
        private IActionResult RedirectBasedOnRole(string rol)
        {
            switch (rol?.ToUpper())
            {
                case "ADMINISTRADOR":
                case "ADMIN":
                    return RedirectToAction("ListarUsuarios", "MantenedorUsuario");

                case "MOZO":
                    return RedirectToAction("Mozo", "Mozo");

                case "COCINA":
                case "COCINERO":
                    // Obtener el EstacionId del usuario
                    var estacionIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EstacionId")?.Value;
                    if (!string.IsNullOrEmpty(estacionIdClaim))
                    {
                        return RedirectToAction("Cocina", "Cocina", new { estacionId = int.Parse(estacionIdClaim) });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // Sin estación asignada
                    }

                default:
                    return RedirectToAction("Dashboard");
            }
        }
    }
}
