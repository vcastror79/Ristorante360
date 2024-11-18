using Ristorante360Admin.Models;
using Ristorante360Admin.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ristorante360Admin.Controllers
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IErrorLoggingService _errorLoggingService;

        public NotificationViewComponent(ApplicationDbContext context, IErrorLoggingService errorLoggingService)
        {
            _context = context;
            _errorLoggingService = errorLoggingService;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                // Fecha límite opcional para filtrar notificaciones (puedes ajustarlo)
                DateTime fechaLimite = DateTime.Now.AddDays(-30);

                // Obtener las notificaciones relevantes
                var notifications = _context.Notifications
                    .Include(n => n.Inventory) // Carga el inventario relacionado
                    .Where(notification => notification.CreatedDate >= fechaLimite)
                    .OrderBy(notification => notification.IsRead) // Ordena no leídas primero
                    .ThenByDescending(notification => notification.CreatedDate) // Luego las más recientes
                    .ToList();

                // Determinar si hay notificaciones no leídas
                ViewBag.HasUnreadNotifications = notifications.Any(notification => !notification.IsRead);

                // Pasar las notificaciones a la vista
                return View(notifications);
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogError("Ocurrió un error al obtener las notificaciones.", ex.ToString());
                // Devuelve una vista vacía en caso de error
                return View(new List<Notification>());
            }
        }
    }
}
