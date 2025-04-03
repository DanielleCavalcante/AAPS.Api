using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Voluntarios;

public interface IVoluntarioService
{
    Task<bool> CriarVoluntario(CriarVoluntarioDto voluntarioDto);
    Task<VoluntarioDto?> ObterVoluntarioPorId(int id);
    Task<Voluntario?> ObterVoluntarioPorUserName(string username); //todo: usar dto
    Task<Voluntario> BuscarVoluntarioPorUsernameETelefone(string username, string telefone); //todo: usar dto
    Task<List<Voluntario>> ObterAdministradores();
    Task<List<string>> ValidarCriacaoVoluntario(CriarVoluntarioDto voluntarioDto);
}