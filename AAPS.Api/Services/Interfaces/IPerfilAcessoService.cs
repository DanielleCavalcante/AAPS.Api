using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Services.Interfaces;

public interface IPerfilAcessoService
{
    Task<IEnumerable<IdentityRole>> ObterPerfis();
    Task CriarPerfil(string nomePerfil);
}
