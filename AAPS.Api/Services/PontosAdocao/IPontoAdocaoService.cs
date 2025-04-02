using AAPS.Api.Dtos.PontoAdocao;

namespace AAPS.Api.Services.PontosAdocao;
public interface IPontoAdocaoService
{
    Task<PontoAdocaoDto> CriarPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto);
    Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocao(FiltroPontoAdocaoDto filtro);
    Task<PontoAdocaoDto?> ObterPontoAdocaoPorId(int id);
    Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocaoAtivos();
    Task<PontoAdocaoDto> AtualizarPontoAdocao(int id, AtualizaPontoAdocaoDto pontoAdocaoDto);
    Task<bool> ExcluirPontoAdocao(int id);
    Task<List<string>> ValidarCriacaoPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto);
    List<string> ValidarAtualizacaoPontoAdocao(AtualizaPontoAdocaoDto pontoAdocaoDto);
}