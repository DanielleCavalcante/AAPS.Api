using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Voluntarios;

public interface IVoluntarioService
{
    Task<bool> CriarVoluntario(CriarVoluntarioDto voluntarioDto);
    Task<bool> RedefinirSenha(int voluntarioId);
    Task<VoluntarioDto?> ObterVoluntarioPorId(int id);
    Task<Voluntario?> ObterVoluntarioPorUserName(string username); //todo: usar dto
    Task<Voluntario> BuscarVoluntarioPorUsernameETelefoneAsync(string username, string telefone); //todo: usar dto
    Task<List<Voluntario>> ObterAdministradoresAsync();
    Task<List<string>> ValidarCriacaoVoluntario(CriarVoluntarioDto voluntarioDto);

    //Task<bool> AlterarSenhaAsync(int voluntarioId, string novaSenha);
}