using AAPS.Api.Dtos.Email;

namespace AAPS.Api.Services.Interfaces;

public interface IAutenticacaoService
{
    Task<bool> RegistrarUsuario(string nomeCompleto, string nomeUsuario, string cpf, string email, string telefone, string senha, string acesso);
    Task<bool> Autenticacao(string nomeUsuario, string senha);
    Task Logout();
    Task<bool> EnviarSolicitacaoRecuperacaoAsync(RecuperarSenhaDto solicitacao);
    Task<(bool Sucesso, string Mensagem)> ResetarSenhaAsync(string nomeUsuario);
    //Task<RespostaRecuperarSenhaDto> RecuperarSenhaPorTelefone(RecuperarSenhaDto dto);
}
