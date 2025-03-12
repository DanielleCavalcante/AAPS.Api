using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AAPS.Api.Services.Impl;

public class PerfilAcessoService : IPerfilAcessoService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly RoleManager<IdentityRole> _roleManager;

    public PerfilAcessoService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    #endregion

    // talvez excluir depois
    public async Task CriarPerfil(string nomePerfil)
    {
        if (!string.IsNullOrEmpty(nomePerfil))
        {
            var perfilExiste = await _roleManager.RoleExistsAsync(nomePerfil);
            if (!perfilExiste)
            {
                await _roleManager.CreateAsync(new IdentityRole(nomePerfil));
            }
        }
    }

    public async Task<IEnumerable<IdentityRole>> ObterPerfis()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<string> ObterIdPorNomeAsync(string nome)
    {
        var perfis = await _roleManager.Roles.ToListAsync();

        var perfilAcesso = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == nome);

        if(perfilAcesso == null)
        {
            return string.Empty;
        }

        return perfilAcesso.Id.ToString();
    }
}
