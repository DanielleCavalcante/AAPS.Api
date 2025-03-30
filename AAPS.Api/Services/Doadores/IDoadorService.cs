using AAPS.Api.Dtos.Doadores;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Doadores;

public interface IDoadorService
{
    Task<DoadorDto> CriarDoador(CriarDoadorDto doadorDto);
    Task<IEnumerable<DoadorDto>> ObterDoadores(FiltroDoadorDto filtro);
    Task<DoadorDto?> ObterDoadorPorId(int id);
    Task<IEnumerable<Doador>> ObterDoadorPorNome(string nome);
    Task<DoadorDto?> AtualizarDoador(int id, AtualizarDoadorDto doadorDto);
    Task<bool> ExcluirDoador(int id);
    Task<List<string>> ValidarCriacaoDoador(CriarDoadorDto doadorDto);
    List<string> ValidarAtualizacaoDoador(AtualizarDoadorDto doadorDto);
}
