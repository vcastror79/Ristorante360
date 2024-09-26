namespace Ristorante360Admin.Services.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailDestino, string token);

    }
}
