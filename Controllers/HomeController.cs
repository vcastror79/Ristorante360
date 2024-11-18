using Microsoft.AspNetCore.Mvc;
using Ristorante360Admin.Models;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ristorante360Admin.Services.Contract;

namespace Ristorante360Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _ristoranteContext;
        private readonly IErrorLoggingService _errorLoggingService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext ristorante360Context, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristorante360Context;
            _logger = logger;
            _errorLoggingService = errorLoggingService;

        }

        public IActionResult Index()
        {
            try
            {

                int cantidadRegistros = _ristoranteContext.Products.Count();
                int cantidadClientes = _ristoranteContext.Clients.Count();
                int cantidadOrdenes = _ristoranteContext.Orders.Count();
                ViewBag.CantidadRegistros = cantidadRegistros;
                ViewBag.CantidadClientes = cantidadClientes;
                ViewBag.CantidadOrdenes = cantidadOrdenes;

                //Guardar la información del nombre de usuario.
                ClaimsPrincipal claimUser = HttpContext.User;
                string nameUser = "";

                if (claimUser.Identity.IsAuthenticated)
                {
                    nameUser = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                ViewData["nameUser"] = nameUser;
                List<Notification> notifications = _ristoranteContext.Notifications.Include(n => n.Inventory).ToList();

                return View(notifications);
            }
            catch (Exception ex)
            {
                string errorMessage = "Se produjo un error al cargar la página.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = errorMessage;
                return View("Error");
            }
        }

        public async Task<IActionResult> LogOff()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {

                string errorMessage = "Se produjo un error al cerrar la sesión.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = errorMessage;
                return View("Error");
            }
        }
        [HttpPost]
        public IActionResult UpdateNotificationIsRead(int notificationId)
        {
            try
            {
                // Buscar la notificación en la base de datos
                var notification = _ristoranteContext.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
                if (notification == null)
                {
                    return NotFound(new { message = "La notificación no fue encontrada." }); // Devolver 404 con un mensaje descriptivo
                }

                // Marcar la notificación como leída
                notification.IsRead = true;
                _ristoranteContext.SaveChanges();

                return Ok(new { success = true, message = "Notificación actualizada correctamente." });
            }
            catch (Exception ex)
            {
                // Registrar el error con un mensaje más detallado
                string errorMessage = $"Error al actualizar la notificación con ID {notificationId}.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Devolver 500 con un mensaje descriptivo
                return StatusCode(500, new { success = false, message = "Error interno del servidor. Por favor, intente nuevamente más tarde." });
            }
        }


    }
}
