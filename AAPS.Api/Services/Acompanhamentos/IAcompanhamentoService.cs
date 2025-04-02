using AAPS.Api.Dtos.Acompanhamento;

namespace AAPS.Api.Services.Acompanhamentos;

public interface IAcompanhamentoService
{
    Task<AcompanhamentoDto> CriarAcompanhamento(CriarAcompanhamentoDto acompanhamentoDto);
    Task<IEnumerable<AcompanhamentoDto>> ObterAcompanhamentos();
    Task<AcompanhamentoDto> ObterAcompanhamentoPorId(int id);
    Task<bool> ExcluirAcompanhamento(int id);
    Task<List<string>> ValidarCriacaoAcompanhamento(CriarAcompanhamentoDto acompanhamentoDto);
}