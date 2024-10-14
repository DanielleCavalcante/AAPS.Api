using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Services.Impl;

public class AutenticacaoService : IAutenticacao
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AutenticacaoService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<bool> Autenticacao(string nomeUsuario, string senha)
    {
        var resultado = await _signInManager
            .PasswordSignInAsync(nomeUsuario, senha, false, lockoutOnFailure: false); 
        // cookie de entrada false (não fica salvo o login depois de sair), não bloquear conta se falhar o login.

        return resultado.Succeeded;
    }

    public async Task<bool> RegistrarUsuario(string nomeUsuario, string senha)
    {
        var appUsuario = new IdentityUser
        {
            UserName = nomeUsuario,
            Email = nomeUsuario
        };

        var resultado = await _userManager.CreateAsync(appUsuario, senha);

        if (resultado.Succeeded)
        {
            await _signInManager.SignInAsync(appUsuario, isPersistent: false); // não persistir cookie de login
        }
        
        return resultado.Succeeded;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}
