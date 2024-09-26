using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ristorante360Admin.Services.Contract
{
    public interface ILogService
    {
        void Log(string detail, string module);

    }

    public class LogService : ILogService
    {
        private readonly RistoranteContext _dbContext; // Usar el tipo de contexto correcto
        private readonly IHttpContextAccessor _httpContextAccessor; // Agrega el campo para acceder a HttpContext

        public LogService(RistoranteContext dbContext, IHttpContextAccessor httpContextAccessor) // Usar el tipo de contexto correcto en el constructor
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Log(string detail, string module)
        {
            TimeZoneInfo zonaHorariaCR = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

            // Obtener la hora actual en UTC
            DateTime horaUtc = DateTime.UtcNow;

            // Convertir la hora UTC a UTC-6
            DateTime horaCST = TimeZoneInfo.ConvertTimeFromUtc(horaUtc, zonaHorariaCR);

            var logEntry = new Log
            {
                Detail = detail,
                Module = module,
                LogDate = horaCST, // Almacenar la hora en UTC-6 (CST)
                UserId = userId.HasValue ? userId.Value : 0 // Aquí, reemplazamos el valor nulo de userId con 0
            };

            _dbContext.Set<Log>().Add(logEntry);
            _dbContext.SaveChanges();
        }
    }
}
