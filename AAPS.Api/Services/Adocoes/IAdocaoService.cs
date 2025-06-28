using AAPS.Api.Dtos.Adocao;

namespace AAPS.Api.Services.Adocoes
{
    public interface IAdocaoService
    {
        Task<AdocaoDto> CriarAdocao(CriarAdocaoDto adocaoDto);
        Task<IEnumerable<AdocaoDto>> ObterAdocoes(FiltroAdocaoDto filtro);
        Task<AdocaoCompletaDto?> ObterAdocaoPorId(int id);
        Task<AdocaoDto> AtualizarAdocao(int id, AtualizarAdocaoDto adocaoDto);
        //Task<bool> ExcluirAdocao(int id);
        Task<AdocaoDto> CancelarAdocao(int id, CancelarAdocaoDto cancelamentoDto);
        Task<List<string>> ValidarCriacaoAdocao(CriarAdocaoDto adocaoDto);
        Task<List<string>> ValidarAtualizacaoAdocao(AtualizarAdocaoDto adocaoDto);
    }
}