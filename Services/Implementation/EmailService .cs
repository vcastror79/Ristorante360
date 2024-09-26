using Ristorante360Admin.Models;
using Ristorante360Admin.Services.Contract;
using System.Net.Mail;

namespace Ristorante360Admin.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly RistoranteContext _ristoranteContext;

        public EmailService(IConfiguration configuration, RistoranteContext ristoranteContext)
        {
            _configuration = configuration;
            _ristoranteContext = ristoranteContext;
        }

        public async Task SendEmailAsync(string emailDestino, string token)
        {
            try
            {
                string EmailOrigen = _configuration["AppSettings:EmailOrigen"];
                string pass = _configuration["AppSettings:Pass"];
                string urlDomain = _configuration["AppSettings:urlDomain"];

                string url = urlDomain + "/User/Recovery/?token=" + token;

                MailMessage oMailMessage = new MailMessage(EmailOrigen, emailDestino, "Recuperación de Contraseña - El Ristorante 360 Admin",
                                "<html>" +
                                "<head>" +
                                "<style>" +
                                "   body { font-family: Arial, sans-serif; }" +
                                "   .container { max-width: 600px; margin: 0 auto; padding: 20px; }" +
                                "   .logo { text-align: center; }" +
                                "   .message { background-color: #f5f5f5; padding: 20px; }" +
                                "   .button { display: block; text-align: center; margin-top: 20px; }" +
                                "</style>" +
                                "</head>" +
                                "<body>" +
                                "<div class='container'>" +
                                "   <div class='message'>" +
                                "       <h2>¡Recuperación de Contraseña!</h2>" +
                                "       <p>Estimado Usuario,</p>" +
                                "       <p>Recibimos una solicitud para restablecer tu contraseña en Ristorante 360.</p>" +
                                "       <p>Haz clic en el siguiente botón para restablecer tu contraseña:</p>" +
                                "       <div class='button'>" +
                                "           <a href='" + url + "' style='background-color: #007BFF; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Recuperar Contraseña</a>" +
                                "       </div>" +
                                "       <p>Si no realizaste esta solicitud, puedes ignorar este correo electrónico.</p>" +
                                "       <p>Atentamente,<br>El Equipo de Ristorante 360 </p>" +
                                "   </div>" +
                                "</div>" +
                                "</body>" +
                                "</html>");


                oMailMessage.IsBodyHtml = true;

                SmtpClient oSmtpClient = new SmtpClient("smtp.office365.com");
                oSmtpClient.EnableSsl = true;
                oSmtpClient.UseDefaultCredentials = false;
                oSmtpClient.Port = 587;
                oSmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, pass);

                await oSmtpClient.SendMailAsync(oMailMessage);
                oSmtpClient.Dispose();
            }
            catch (Exception ex)
            {
                await SaveErrorToDatabase(ex);
            }
        }
        private async Task SaveErrorToDatabase(Exception ex)
        {
            var error = new Error
            {
                ErrorMessage = ex.Message,
                DateTime = DateTime.Now
            };

            _ristoranteContext.Errors.Add(error);
            await _ristoranteContext.SaveChangesAsync();
        }

    }
}
