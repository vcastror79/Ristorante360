using System;

namespace Ristorante360Admin.Services.Contract
{
    public interface IErrorLoggingService
    {
        void LogError(string errorMessage, string exceptionMessage);
    }
}
