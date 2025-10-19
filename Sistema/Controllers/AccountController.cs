using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Sistema.Controllers
{
    public class AccountController : Controller
    {
        //// GET: Account/Index (Login)
        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //    // Si ya está autenticado, redirigir al dashboard principal
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("ListarUsuarios", "MantenedorUsuario");
        //    }
        //    return View();
        //}

        //// POST: Account/Index (Procesar Login)
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(string username, string password, bool rememberMe = false)
        //{
        //    try
        //    {
        //        // Validación de campos vacíos
        //        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        //        {
        //            ViewBag.Error = "Usuario y contraseña son obligatorios";
        //            return View();
        //        }

        //        // Autenticar usuario usando la capa lógica
        //        var usuario = logUsuario.Instancia.Autenticar(username, password);

        //        // Verificar si el usuario existe y está activo
        //        if (usuario == null)
        //        {
        //            ViewBag.Error = "Usuario o contraseña incorrectos";
        //            return View();
        //        }

        //        if (!usuario.Estado)
        //        {
        //            ViewBag.Error = "Su cuenta está inactiva. Contacte al administrador";
        //            return View();
        //        }

        //        // Crear los claims del usuario
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
        //            new Claim(ClaimTypes.Name, usuario.UserName),
        //            new Claim(ClaimTypes.Role, usuario.Rol),
        //            new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
        //            new Claim("UserId", usuario.UsuarioId.ToString())
        //        };

        //        // Agregar claims de estación si existen
        //        if (usuario.EstacionId.HasValue)
        //        {
        //            claims.Add(new Claim("EstacionId", usuario.EstacionId.Value.ToString()));

        //            if (!string.IsNullOrEmpty(usuario.EstacionNombre))
        //            {
        //                claims.Add(new Claim("EstacionNombre", usuario.EstacionNombre));
        //            }
        //        }

        //        // Crear la identidad del usuario
        //        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        // Configurar las propiedades de autenticación
        //        var authProperties = new AuthenticationProperties
        //        {
        //            IsPersistent = rememberMe, // Mantener sesión si se marcó "Recordar usuario"
        //            ExpiresUtc = rememberMe
        //                ? DateTimeOffset.UtcNow.AddDays(30)
        //                : DateTimeOffset.UtcNow.AddHours(24),
        //            AllowRefresh = true,
        //            IssuedUtc = DateTimeOffset.UtcNow
        //        };

        //        // Iniciar sesión
        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(claimsIdentity),
        //            authProperties);

        //        // Log para debug
        //        System.Diagnostics.Debug.WriteLine($"Login exitoso: {usuario.UserName} - Rol: {usuario.Rol}");

        //        // Redirigir según el rol
        //        return RedirectBasedOnRole(usuario.Rol);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log del error
        //        System.Diagnostics.Debug.WriteLine($"Error en login: {ex.Message}");
        //        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

        //        ViewBag.Error = "Error al iniciar sesión. Por favor, intente nuevamente.";
        //        return View();
        //    }
        //}

        //// Método auxiliar para redirigir según el rol
        //private IActionResult RedirectBasedOnRole(string rol)
        //{
        //    switch (rol?.ToUpper())
        //    {
        //        case "ADMINISTRADOR":
        //        case "ADMIN":
        //            return RedirectToAction("ListarUsuarios", "MantenedorUsuario");

        //        case "OPERADOR":
        //            return RedirectToAction("Index", "Home");

        //        case "SUPERVISOR":
        //            return RedirectToAction("Index", "Home");

        //        default:
        //            return RedirectToAction("Index", "Home");
        //    }
        //}

        //// POST: Account/Logout
        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    try
        //    {
        //        // Log para debug
        //        System.Diagnostics.Debug.WriteLine($"Usuario cerrando sesión: {User.Identity.Name}");

        //        // Cerrar sesión
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //        return RedirectToAction("Index", "Account");
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error en logout: {ex.Message}");
        //        return RedirectToAction("Index", "Account");
        //    }
        //}

        //// GET: Account/AccessDenied
        //[AllowAnonymous]
        //public IActionResult AccessDenied()
        //{
        //    ViewBag.Error = "No tiene permisos para acceder a este recurso";
        //    return View("Index");
        //}
    }
}
