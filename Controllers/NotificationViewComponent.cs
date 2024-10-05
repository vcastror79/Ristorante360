using Ristorante360Admin.Models;
using Ristorante360Admin.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ristorante360Admin.Controllers
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IErrorLoggingService _errorLoggingService; // Agrega la inyección del servicio de registro de errores

        public NotificationViewComponent(ApplicationDbContext context, IErrorLoggingService errorLoggingService)
        {
            _context = context;
            _errorLoggingService = errorLoggingService; // Asigna el servicio de registro de errores
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                DateTime fechaLimite = DateTime.Now.AddDays(-2);

                var notifications = _context.Notifications
                    .Include(n => n.Inventory)
                    .Where(notification => notification.CreatedDate >= fechaLimite)
                    .OrderBy(notification => notification.IsRead)
                    .ThenBy(notification => notification.CreatedDate)
                    .ToList();

                var hasUnreadNotifications = notifications.Any(notification => !notification.IsRead);

                ViewBag.HasUnreadNotifications = hasUnreadNotifications;

                return View(notifications);
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogError("Ocurrió un error al obtener las notificaciones.", ex.ToString());
                throw; // Lanza la excepción nuevamente para que pueda ser manejada en el nivel superior si es necesario
            }
        }
    }
}
