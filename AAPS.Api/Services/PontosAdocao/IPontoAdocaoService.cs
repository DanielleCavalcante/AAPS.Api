using AAPS.Api.Dtos.PontoAdocao;
using AAPS.Api.Dtos.PontosAdocao;
using AAPS.Api.Models;

namespace AAPS.Api.Services.PontosAdocao;
public interface IPontoAdocaoService
{
    Task<PontoAdocaoDto> CriarPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto);
    Task<IEnumerable<PontoAdocao>> ObterPontosAdocao();
    Task<PontoAdocaoDto?> ObterPontoAdocaoPorId(int id);
    Task<IEnumerable<PontoAdocao>> ObterPontoAdocaoPorNome(string nome);
    Task<PontoAdocaoDto> AtualizarPontoAdocao(int id, AtualizaPontoAdocaoDto pontoAdocaoDto);
    Task<bool> ExcluirPontoAdocao(int id);
}
