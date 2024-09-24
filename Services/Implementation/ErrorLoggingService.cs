using ElChanteAdmin.Models;
using ElChanteAdmin.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElChanteAdmin.Services.Implementation
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        private readonly ElChanteContext _elChanteContext;

        public ErrorLoggingService(ElChanteContext elChanteContext)
        {
            _elChanteContext = elChanteContext;
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

            _elChanteContext.Errors.Add(errorLog);
            _elChanteContext.SaveChanges();
        }
    }
}
