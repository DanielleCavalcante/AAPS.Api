using Microsoft.AspNetCore.Identity;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Impl;

public class PerfilAcessoService : IPerfilAcessoService
{
    private readonly RoleManager<IdentityRole> _perfilAcesso;

    public PerfilAcessoService(RoleManager<IdentityRole> perfilAcesso)
    {
        _perfilAcesso = perfilAcesso;
    }

    public async Task<IEnumerable<IdentityRole>> ObterPerfis()
    {
        return await _perfilAcesso.Roles.ToListAsync();
    }

    // talvez excluir depois
    public async Task CriarPerfil(string nomePerfil)
    {
        if (!string.IsNullOrEmpty(nomePerfil))
        {
            var perfilExiste = await _perfilAcesso.RoleExistsAsync(nomePerfil);
            if (!perfilExiste)
            {
                await _perfilAcesso.CreateAsync(new IdentityRole(nomePerfil));
            }
        }
    }
}
