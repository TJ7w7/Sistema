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
            HttpContext.Session?.Clear();

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

                //_logger.LogInformation($"Login exitoso: {usuario.UserName} - Rol: {usuario.Rol}");

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

                default:
                    return RedirectToAction("Dashboard");
            }
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("ListarUsuarios", "MantenedorUsuario");
        //    }
        //    return View();
        //}

        //// POST: Home/Index - Procesar Login CON DEBUG
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(string username, string password, bool rememberMe = false)
        //{
        //    try
        //    {
        //        // 1. LOG: Verificar que llegan los datos
        //        Debug.WriteLine("=== INICIO DEBUG LOGIN ===");
        //        Debug.WriteLine($"Username recibido: '{username}'");
        //        Debug.WriteLine($"Password recibido (primeros 3 chars): '{password?.Substring(0, Math.Min(3, password?.Length ?? 0))}***'");
        //        Debug.WriteLine($"RememberMe: {rememberMe}");

        //        // Validación de campos vacíos
        //        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        //        {
        //            ViewBag.Error = "Usuario y contraseña son obligatorios";
        //            Debug.WriteLine("ERROR: Campos vacíos");
        //            return View();
        //        }

        //        // 2. LOG: Verificar encriptación
        //        string passwordEncriptado = Recursos.EncriptarSHA256(password);
        //        Debug.WriteLine($"Password encriptado (SHA256): {passwordEncriptado}");

        //        // 3. LOG: Intentar autenticar
        //        Debug.WriteLine("Llamando a logUsuario.Instancia.Autenticar...");
        //        var usuario = logUsuario.Instancia.Autenticar(username, password);

        //        // 4. LOG: Resultado de autenticación
        //        if (usuario == null)
        //        {
        //            Debug.WriteLine("ERROR: Usuario NULL - Credenciales incorrectas o usuario no existe");

        //            // DEBUG ADICIONAL: Verificar si el usuario existe en la BD
        //            try
        //            {
        //                var todosUsuarios = logUsuario.Instancia.ListarUsuarios();
        //                Debug.WriteLine($"Total usuarios en BD: {todosUsuarios.Count}");

        //                var usuarioEncontrado = todosUsuarios.FirstOrDefault(u =>
        //                    u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        //                if (usuarioEncontrado != null)
        //                {
        //                    Debug.WriteLine($"Usuario '{username}' SÍ existe en BD");
        //                    Debug.WriteLine($"Password en BD: {usuarioEncontrado.Pass}");
        //                    Debug.WriteLine($"Password ingresado (encriptado): {passwordEncriptado}");
        //                    Debug.WriteLine($"¿Passwords coinciden?: {usuarioEncontrado.Pass == passwordEncriptado}");
        //                    Debug.WriteLine($"Estado del usuario: {usuarioEncontrado.Estado}");
        //                }
        //                else
        //                {
        //                    Debug.WriteLine($"Usuario '{username}' NO existe en BD");
        //                    Debug.WriteLine("Usuarios disponibles:");
        //                    foreach (var u in todosUsuarios)
        //                    {
        //                        Debug.WriteLine($"  - {u.UserName} (Estado: {u.Estado})");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine($"Error al listar usuarios para debug: {ex.Message}");
        //            }

        //            ViewBag.Error = "Usuario o contraseña incorrectos";
        //            return View();
        //        }

        //        Debug.WriteLine($"Usuario autenticado: {usuario.UserName}");
        //        Debug.WriteLine($"Rol: {usuario.Rol}");
        //        Debug.WriteLine($"Estado: {usuario.Estado}");

        //        // 5. Verificar estado
        //        if (!usuario.Estado)
        //        {
        //            Debug.WriteLine("ERROR: Usuario inactivo");
        //            ViewBag.Error = "Su cuenta está inactiva. Contacte al administrador";
        //            return View();
        //        }

        //        // 6. Crear claims
        //        Debug.WriteLine("Creando claims...");
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
        //            new Claim(ClaimTypes.Name, usuario.UserName),
        //            new Claim(ClaimTypes.Role, usuario.Rol),
        //            new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
        //            new Claim("UserId", usuario.UsuarioId.ToString())
        //        };

        //        if (usuario.EstacionId.HasValue)
        //        {
        //            claims.Add(new Claim("EstacionId", usuario.EstacionId.Value.ToString()));

        //            if (!string.IsNullOrEmpty(usuario.EstacionNombre))
        //            {
        //                claims.Add(new Claim("EstacionNombre", usuario.EstacionNombre));
        //            }
        //        }

        //        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        var authProperties = new AuthenticationProperties
        //        {
        //            IsPersistent = rememberMe,
        //            ExpiresUtc = rememberMe
        //                ? DateTimeOffset.UtcNow.AddDays(30)
        //                : DateTimeOffset.UtcNow.AddHours(24),
        //            AllowRefresh = true
        //        };

        //        // 7. Iniciar sesión
        //        Debug.WriteLine("Iniciando sesión...");
        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(claimsIdentity),
        //            authProperties);

        //        Debug.WriteLine("Sesión iniciada correctamente");
        //        _logger.LogInformation($"Login exitoso: {usuario.UserName} - Rol: {usuario.Rol}");
        //        Debug.WriteLine("=== FIN DEBUG LOGIN ===");

        //        // Redirigir según el rol
        //        return RedirectBasedOnRole(usuario.Rol);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"EXCEPCIÓN en login: {ex.Message}");
        //        Debug.WriteLine($"StackTrace: {ex.StackTrace}");
        //        _logger.LogError($"Error en login: {ex.Message}");
        //        ViewBag.Error = "Error al iniciar sesión. Por favor, intente nuevamente.";
        //        return View();
        //    }
        //}

        //[Authorize]
        //public IActionResult Dashboard()
        //{
        //    var userName = User.Identity.Name;
        //    var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
        //    var rol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        //    var estacionNombre = User.Claims.FirstOrDefault(c => c.Type == "EstacionNombre")?.Value;

        //    ViewBag.UserName = userName;
        //    ViewBag.FullName = fullName ?? userName;
        //    ViewBag.Rol = rol;
        //    ViewBag.EstacionNombre = estacionNombre ?? "Sin estación asignada";

        //    return View();
        //}

        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    try
        //    {
        //        Debug.WriteLine($"Usuario cerrando sesión: {User.Identity.Name}");
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error en logout: {ex.Message}");
        //        return RedirectToAction("Index");
        //    }
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //private IActionResult RedirectBasedOnRole(string rol)
        //{
        //    switch (rol?.ToUpper())
        //    {
        //        case "ADMINISTRADOR":
        //        case "ADMIN":
        //            return RedirectToAction("ListarUsuarios", "MantenedorUsuario");

        //        case "OPERADOR":
        //        case "SUPERVISOR":
        //            return RedirectToAction("Dashboard");

        //        default:
        //            return RedirectToAction("Dashboard");
        //    }
        //}
    }
}
