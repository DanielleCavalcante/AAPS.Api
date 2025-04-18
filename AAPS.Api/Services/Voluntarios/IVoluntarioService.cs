using AAPS.Api.Dtos.Voluntario;

namespace AAPS.Api.Services.Voluntarios;

public interface IVoluntarioService
{
    Task<bool> CriarVoluntario(CriarVoluntarioDto voluntarioDto);
    Task<IEnumerable<VoluntarioDto>> ObterVoluntarios(FiltroVoluntarioDto filtro);
    Task<VoluntarioDto?> ObterVoluntarioPorId(int id);
    Task<IEnumerable<VoluntarioDto>> ObterVoluntariosAtivos();
    Task<VoluntarioDto?> ObterVoluntarioPorUsernameETelefone(string username, string telefone);
    Task<List<VoluntarioDto>> ObterAdministradores();
    Task<VoluntarioDto?> ObterVoluntarioPorUserName(string username);
    Task<VoluntarioDto> AtualizarVoluntario(int id, AtualizarVoluntarioDto voluntarioDto);
    Task<bool> ExcluirVoluntario(int id);
    Task<List<string>> ValidarCriacaoVoluntario(CriarVoluntarioDto voluntarioDto);
    List<string> ValidarAtualizacaoVoluntario(AtualizarVoluntarioDto voluntarioDto);
}