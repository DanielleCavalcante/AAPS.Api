using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Services.Interfaces;

public interface IPerfilAcessoService
{
    Task CriarPerfil(string nomePerfil);
    Task<IEnumerable<IdentityRole>> ObterPerfis();
    Task<string> ObterIdPorNomeAsync(string nome);
}
