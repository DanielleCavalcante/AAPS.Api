using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Adotantes;

public interface IAdotanteService
{
    Task<IEnumerable<Adotante>> ObterAdotantes();
    Task<Adotante> ObterAdotantePorId(int id);
    Task<IEnumerable<Adotante>> ObterAdotantePorNome(string nome);
    Task CriarAdotante(AdotanteDto adotanteDto);
    Task AtualizarAdotante(int id, AdotanteDto AdotanteDto);
    Task ExcluirAdotante(int id);
}
