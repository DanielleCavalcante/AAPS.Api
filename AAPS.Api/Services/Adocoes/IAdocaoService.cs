using AAPS.Api.Dtos.Adocao;
using AAPS.Api.Dtos.PontoAdocao;

namespace AAPS.Api.Services.Adocoes
{
    public interface IAdocaoService
    {
        Task<AdocaoDto> CriarAdocao(CriarAdocaoDto adocaoDto);
        Task<IEnumerable<AdocaoDto>> ObterAdocoes(FiltroAdocaoDto filtro);
        Task<AdocaoDto?> ObterAdocaoPorId(int id);
        Task<AdocaoDto> AtualizarAdocao(int id, AtualizarAdocaoDto adocaoDto);
        Task<bool> ExcluirAdocao(int id);
        Task<List<string>> ValidarCriacaoAdocao(CriarAdocaoDto adocaoDto);
        Task<List<string>> ValidarAtualizacaoAdocao(AtualizarAdocaoDto adocaoDto);
    }
}