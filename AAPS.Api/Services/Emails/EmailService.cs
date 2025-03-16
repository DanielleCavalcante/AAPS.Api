using System.Net;
using System.Net.Mail;

namespace AAPS.Api.Services
{
    public class EmailService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        public async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string mensagem)
        {
            try
            {
                string emailOrigem = _configuration["EmailSettings:Email"];
                string senha = _configuration["EmailSettings:Senha"];
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;

                var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(emailOrigem, senha),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailOrigem),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(destinatario);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
