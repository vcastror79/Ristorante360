using Ristorante360.Models;
using Ristorante360.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Ristorante360.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



namespace Ristorante360.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly RistoranteContext _elChanteContext; // Asegúrate de usar este nombre consistentemente
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;

        // Constructor corregido
        public ClientesController(RistoranteContext elChanteContext, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _elChanteContext = elChanteContext; // Corregir la asignación, asegurarse de usar el nombre correcto
            _logService = logService;
            _errorLoggingService = errorLoggingService;
        }

        public IActionResult ClientList()
        {
            try
            {
                List<Client> listClient = _elChanteContext.Clients
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
