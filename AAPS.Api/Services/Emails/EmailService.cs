using AAPS.Api.Services.TermoAdocao;
using System.Net;
using System.Net.Mail;

namespace AAPS.Api.Services
{
    public class EmailService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IConfiguration _configuration;
        private readonly ITermoAdocaoService _termoAdocaoService;

        public EmailService(IConfiguration configuration, ITermoAdocaoService termoAdocaoService)
        {
            _configuration = configuration;
            _termoAdocaoService = termoAdocaoService;
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

        public async Task<bool> EnviarEmailComAnexoAsync(string destinatario, string assunto, string mensagem, int adocaoId)
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

                var pdfBytes = await _termoAdocaoService.GerarPdf(adocaoId);

                using (var pdfStream = new MemoryStream(pdfBytes))
                {
                    var attachment = new Attachment(pdfStream, "termo_adocao_responsavel.pdf", "application/pdf");

                    mailMessage.Attachments.Add(attachment);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
