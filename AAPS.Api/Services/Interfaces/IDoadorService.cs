using AAPS.Api.Dtos;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Interfaces;

public interface IDoadorService
{
    Task<IEnumerable<Doador>> ObterDoadores();
    Task<Doador> ObterDoadorPorId(int id);
    Task<IEnumerable<Doador>> ObterDoadorPorNome(string nome);
    Task CriarDoador(DoadorDto doadorDto);
    Task AtualizarDoador(int id, DoadorDto doadorDto);
    Task ExcluirDoador(int id);
}
