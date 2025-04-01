using AAPS.Api.Dtos.Autenticacao;

namespace AAPS.Api.Services.Autenticacao;

public interface IAutenticacaoService
{
    Task<bool> Autenticacao(LoginDto login);
    Task<TokenDto> LoginComToken(LoginDto infoUsuario);
    Task Logout();

    //Task<string> GerarCodigoRecuperacao(int voluntarioId);
    //Task SalvarCodigoRecuperacaoAsync(int voluntarioId, string codigo);
    //Task<bool> ValidarCodigoRecuperacao(int voluntarioId, string codigo);
}
