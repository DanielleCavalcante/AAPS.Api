using AAPS.Api.Dtos.Autenticacao;

namespace AAPS.Api.Services.Autenticacao;

public interface IAutenticacaoService
{
    Task<bool> Autenticacao(LoginDto login);
    Task<TokenDto> LoginComToken(LoginDto infoUsuario);
    Task Logout();
}
