namespace AAPS.Api.Services.Interfaces;

public interface IAutenticacao
{
    Task<bool> RegistrarUsuario(string nomeUsuario, string senha);
    Task<bool> Autenticacao(string nomeUsuario, string senha);
    Task Logout();
    
}
