using Microsoft.AspNetCore.Mvc;
using Ristorante360Admin.Models;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ristorante360Admin.Services.Contract;

namespace Ristorante360.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RistoranteContext _ristoranteContext;
        private readonly IErrorLoggingService _errorLoggingService;

        public HomeController(ILogger<HomeController> logger, RistoranteContext ristorante360Context, IErrorLoggingService errorLoggingService)
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
                var notification = _ristoranteContext.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    _ristoranteContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound(); // Devolver un resultado 404 si no se encuentra la notificación
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al actualizar la notificación.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                return StatusCode(500); // Devolver un resultado 500 en caso de error interno del servidor
            }
        }

    }
}
