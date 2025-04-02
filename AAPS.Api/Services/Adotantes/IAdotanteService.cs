using AAPS.Api.Dtos.Adotante;

namespace AAPS.Api.Services.Adotantes;

public interface IAdotanteService
{
    Task<AdotanteDto> CriarAdotante(CriarAdotanteDto adotanteDto);
    Task<IEnumerable<AdotanteDto>> ObterAdotantes(FiltroAdotanteDto filtro);
    Task<AdotanteDto?> ObterAdotantePorId(int id);
    Task<IEnumerable<AdotanteDto>> ObterAdotantesAtivos();
    Task<AdotanteDto?> AtualizarAdotante(int id, AtualizarAdotanteDto adotanteDto);
    Task<bool> ExcluirAdotante(int id);
    Task<List<string>> ValidarCriacaoAdotante(CriarAdotanteDto adotanteDto);
    List<string> ValidarAtualizacaoAdotante(AtualizarAdotanteDto adotanteDto);
}