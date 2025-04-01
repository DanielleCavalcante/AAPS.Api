using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Voluntarios;

public interface IVoluntarioService
{
    Task<bool> RegistrarVoluntario(CriarVoluntarioDto voluntarioDto);
    Task<bool> RedefinirSenha(int voluntarioId);
    Task<Voluntario?> ObterVoluntarioPorId(int id);
    Task<Voluntario?> ObterVoluntarioPorUserName(string username);
    Task<Voluntario> BuscarVoluntarioPorUsernameETelefoneAsync(string username, string telefone);
    Task<List<Voluntario>> ObterAdministradoresAsync();
    Task<List<string>> ValidarCriacaoVoluntario(CriarVoluntarioDto voluntarioDto);

    //Task<bool> AlterarSenhaAsync(int voluntarioId, string novaSenha);
}
