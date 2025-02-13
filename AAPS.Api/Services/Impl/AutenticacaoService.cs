using AAPS.Api.Context;
using AAPS.Api.Dtos;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Services.Impl;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPerfilAcessoService _perfilAcesso;
    private readonly IVoluntarioService _voluntarioService;
    //private readonly IVo
    AppDbContext _context;

    public AutenticacaoService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AppDbContext context, IPerfilAcessoService perfilAcesso, IVoluntarioService voluntarioService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _perfilAcesso = perfilAcesso;
        _voluntarioService = voluntarioService;
    }

    public async Task<bool> RegistrarUsuario(string nomeCompleto, string nomeUsuario, string cpf, string email, string telefone, string senha, string acesso) //string status
    {
        var appUsuario = new IdentityUser
        {
            UserName = nomeUsuario,
            Email = email,
            PhoneNumber = telefone,
        };

        var resultadoIdentity = await _userManager.CreateAsync(appUsuario, senha);

        var appUsuarioVoluntario = new VoluntarioDto
        {
            Nome = nomeCompleto,
            Cpf = cpf,
            Status = StatusEnum.Ativo,
            RedefinirSenha = true,
            IdentityUserId = appUsuario.Id,
            IdentityRoleId = "1",
        };

        var resultadoVoluntario = _voluntarioService.RegistrarVoluntario(appUsuarioVoluntario);

        if (resultadoIdentity.Succeeded && resultadoVoluntario.IsCompletedSuccessfully)
        {
            await _signInManager.SignInAsync(appUsuario, isPersistent: false); // não persistir cookie de login
        }

        return resultadoIdentity.Succeeded;
    }

    public async Task<bool> Autenticacao(string nomeUsuario, string senha)
    {
        var resultado = await _signInManager
            .PasswordSignInAsync(nomeUsuario, senha, false, lockoutOnFailure: false);
        // cookie de entrada false (não fica salvo o login depois de sair), não bloquear conta se falhar o login.

        return resultado.Succeeded;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    // METODOS PRIVADOS

    private string ObterIdAcessoPerfil(string acesso)
    {
        var perfis = _perfilAcesso.ObterPerfis();

        if (acesso == "Admin")
        {
            return null;
        }
        else
        {
            return acesso;
        }
    }
}
