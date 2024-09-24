namespace Ristorante360.Services.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailDestino, string token);

    }
}
