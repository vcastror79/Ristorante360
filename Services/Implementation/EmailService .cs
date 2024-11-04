using Ristorante360Admin.Services.Contract;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Ristorante360Admin.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string emailDestino, string token)
        {
            try
            {
                // Leer los valores desde la sección EmailSettings en appsettings.json
                string smtpServer = _configuration["EmailSettings:SmtpServer"];
                int port = int.Parse(_configuration["EmailSettings:Port"]);
                string senderEmail = _configuration["EmailSettings:SenderEmail"];
                string senderName = _configuration["EmailSettings:SenderName"];
                string username = _configuration["EmailSettings:Username"];
                string password = _configuration["EmailSettings:Password"];
                bool enableSSL = bool.Parse(_configuration["EmailSettings:EnableSSL"]);
                string urlDomain = _configuration["EmailSettings:urlDomain"]; // Asegúrate de tener este valor en appsettings.json

                // Crear el enlace de recuperación con el token
                string url = urlDomain + "/User/Recovery/?token=" + token;

                // Preparar el mensaje de correo
                MailMessage oMailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Recuperación de Contraseña - El Ristorante 360 Admin",
                    Body = $"<html><body><p>Para recuperar tu contraseña, haz clic en el siguiente enlace:</p><a href='{url}'>Recuperar Contraseña</a></body></html>",
                    IsBodyHtml = true
                };
                oMailMessage.To.Add(emailDestino);

                // Configurar el cliente SMTP
                SmtpClient oSmtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new System.Net.NetworkCredential(username, password),
                    EnableSsl = enableSSL
                };

                // Enviar el correo
                await oSmtpClient.SendMailAsync(oMailMessage);
                oSmtpClient.Dispose();
            }
            catch (Exception ex)
            {
                // Lógica para guardar errores en la base de datos o manejar excepciones
                await SaveErrorToDatabase(ex);
            }
        }

        private async Task SaveErrorToDatabase(Exception ex)
        {
            // Guardar los errores en la base de datos para facilitar la depuración
            // Implementa la lógica para guardar el error si es necesario
            Console.WriteLine("Error al enviar correo: " + ex.Message);
        }
    }
}
