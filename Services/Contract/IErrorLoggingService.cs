using System;

namespace Ristorante360.Services.Contract
{
    public interface IErrorLoggingService
    {
        void LogError(string errorMessage, string exceptionMessage);
    }
}
