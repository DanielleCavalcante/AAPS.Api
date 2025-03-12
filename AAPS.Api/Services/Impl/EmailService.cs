using AAPS.Api.Dtos.Email;
using System.Net;
using System.Net.Mail;

namespace AAPS.Api.Services.Impl;

public class EmailService
{
    private readonly IConfiguration _configuracao;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuracao, ILogger<EmailService> logger)
    {
        _configuracao = configuracao;
        _logger = logger;
    }

    public async Task<bool> EnviarEmailAsync(EmailDto emailDto)
    {
        try
        {
            var smtpConfig = _configuracao.GetSection("SmtpConfiguracao");

            using var smtpClient = new SmtpClient(smtpConfig["Servidor"])
            {
                Port = int.Parse(smtpConfig["Porta"]),
                Credentials = new NetworkCredential(smtpConfig["Usuario"], smtpConfig["Senha"]),
                EnableSsl = bool.Parse(smtpConfig["UsarSSL"])
            };

            var mensagemEmail = new MailMessage
            {
                From = new MailAddress(smtpConfig["EmailRemetente"]),
                Subject = emailDto.Assunto,
                Body = emailDto.Mensagem,
                IsBodyHtml = true
            };

            mensagemEmail.To.Add(emailDto.Destinatario);

            await smtpClient.SendMailAsync(mensagemEmail);
            _logger.LogInformation($"E-mail enviado para {emailDto.Destinatario}.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao enviar e-mail: {ex.Message}");
            return false;
        }
    }
}
