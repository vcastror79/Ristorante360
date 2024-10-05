using Ristorante360Admin.Models;
using Ristorante360Admin.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ristorante360Admin.Services.Implementation
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        private readonly ApplicationDbContext _ristoranteContext;

        public ErrorLoggingService(ApplicationDbContext ristoranteContext)
        {
            _ristoranteContext = ristoranteContext;
        }

        public void LogError(string errorMessage, string exceptionMessage)
        {
            // Registrar el error en la base de datos
            var errorLog = new Error
            {
                DateTime = DateTime.Now,
                ErrorMessage = errorMessage,
                ExceptionMessage = exceptionMessage,
            };

            _ristoranteContext.Errors.Add(errorLog);
            _ristoranteContext.SaveChanges();
        }
    }
}
