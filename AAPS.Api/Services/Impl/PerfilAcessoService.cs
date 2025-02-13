using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AAPS.Api.Services.Impl;

public class PerfilAcessoService : IPerfilAcessoService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public PerfilAcessoService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<IdentityRole>> ObterPerfis()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<string> ObterIdPorNomeAsync(string nome)
    {
        var perfis = await _roleManager.Roles.ToListAsync();

        var perfilAcesso = perfis.FirstOrDefault(x => x.Name == nome);

        if (perfilAcesso == null)
        {
            throw new Exception($"Perfil {nome} não encontrado.");
        }

        return perfilAcesso.Id;
    }

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
}
