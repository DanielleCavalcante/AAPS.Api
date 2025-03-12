using AAPS.Api.Dtos.Doadores;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Interfaces;

public interface IDoadorService
{
    Task<DoadorDto> CriarDoador(DoadorDto doadorDto);
    Task<IEnumerable<Doador>> ObterDoadores();
    Task<DoadorDto?> ObterDoadorPorId(int id);
    Task<IEnumerable<Doador>> ObterDoadorPorNome(string nome);
    Task<DoadorDto?> AtualizarDoador(int id, DoadorDto doadorDto);
    Task<bool> ExcluirDoador(int id);
}
