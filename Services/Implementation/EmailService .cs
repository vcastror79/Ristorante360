using ElChanteAdmin.Models;
using ElChanteAdmin.Services.Contract;
using System.Net.Mail;

namespace ElChanteAdmin.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ElChanteContext _elChanteContext;

        public EmailService(IConfiguration configuration, ElChanteContext elChanteContext)
        {
            _configuration = configuration;
            _elChanteContext = elChanteContext;
        }

        public async Task SendEmailAsync(string emailDestino, string token)
        {
            try
            {
                string EmailOrigen = _configuration["AppSettings:EmailOrigen"];
                string pass = _configuration["AppSettings:Pass"];
                string urlDomain = _configuration["AppSettings:urlDomain"];

                string url = urlDomain + "/User/Recovery/?token=" + token;

                MailMessage oMailMessage = new MailMessage(EmailOrigen, emailDestino, "Recuperación de Contraseña - El Chante Admin",
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
                                "       <p>Recibimos una solicitud para restablecer tu contraseña en El Chante Admin.</p>" +
                                "       <p>Haz clic en el siguiente botón para restablecer tu contraseña:</p>" +
                                "       <div class='button'>" +
                                "           <a href='" + url + "' style='background-color: #007BFF; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Recuperar Contraseña</a>" +
                                "       </div>" +
                                "       <p>Si no realizaste esta solicitud, puedes ignorar este correo electrónico.</p>" +
                                "       <p>Atentamente,<br>El Equipo de El Chante</p>" +
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

            _elChanteContext.Errors.Add(error);
            await _elChanteContext.SaveChangesAsync();
        }

    }
}
