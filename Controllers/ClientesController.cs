using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels;
using Ristorante360Admin.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ristorante360Admin.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _ristoranteContext; // Asegúrate de usar este nombre consistentemente
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;

        // Constructor modificado
        public ClientesController(ApplicationDbContext ristoranteContext, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristoranteContext; 
            _logService = logService;
            _errorLoggingService = errorLoggingService;
        }

        public IActionResult ClientList()
        {
            try
            {
                List<Client> listClient = _ristoranteContext.Clients
                    .Where(client => client.status == true)
                    .ToList();
                return View(listClient);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener la lista de clientes.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al obtener la lista de clientes.";
                return View(); // o redirigir a una vista de error personalizada
            }
        }

        // Otros métodos sin cambios...
    }
}
