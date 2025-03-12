using AAPS.Api.Context;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Dtos.Email;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Impl;

public class AutenticacaoService : IAutenticacaoService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPerfilAcessoService _perfilAcesso;
    private readonly IVoluntarioService _voluntarioService;
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;

    public AutenticacaoService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AppDbContext context, IPerfilAcessoService perfilAcesso, IVoluntarioService voluntarioService, EmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _perfilAcesso = perfilAcesso;
        _voluntarioService = voluntarioService;
        _emailService = emailService;
    }

    #endregion

    public async Task<bool> RegistrarUsuario(string nomeCompleto, string nomeUsuario, string cpf, string email, string telefone, string senha, string acesso) //string status
    {
        var voluntario = new Voluntario
        {
            UserName = nomeUsuario,
            Email = email,
            PhoneNumber = telefone,
            NomeCompleto = nomeCompleto,
            Cpf = cpf,
            Status = StatusEnum.Ativo
        };

        var resultado = await _userManager.CreateAsync(voluntario, senha);

        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(voluntario, acesso);
            await _signInManager.SignInAsync(voluntario, isPersistent: false); // não persistir cookie de login
        }

        return resultadoIdentity.Succeeded;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> Autenticacao(string nomeUsuario, string senha)
    {
        var resultado = await _signInManager
            .PasswordSignInAsync(nomeUsuario, senha, false, lockoutOnFailure: false);
        // cookie de entrada false (não fica salvo o login depois de sair), não bloquear conta se falhar o login.

        return resultado.Succeeded;
    }

    public async Task<bool> EnviarSolicitacaoRecuperacaoAsync(RecuperarSenhaDto dto)
    {
        bool usuarioValido = VerificarUsuarioNoBanco(dto.NomeUsuario, dto.Telefone);

        if (!usuarioValido)
        {
            return false;
        }

        var email = new EmailDto
        {
            Destinatario = "s.daniellecavalcante@gmail.com",
            Assunto = "Solicitação de Redefinição de Senha",
            Mensagem = $"O usuário {dto.NomeUsuario} com telefone {dto.Telefone} solicitou redefinição de senha."
        };

        return await _emailService.EnviarEmailAsync(email);
    }

    // caso tenha uma tela de resetar senha:
    public async Task<(bool Sucesso, string Mensagem)> ResetarSenhaAsync(string nomeUsuario)
    {
        var usuario = await _userManager.FindByNameAsync(nomeUsuario);

        if (usuario == null)
        {
            return (false, "Usuário não encontrado.");
        }

        var voluntario = await _context.Voluntarios.FirstOrDefaultAsync(v => v.IdentityUserId == usuario.Id);

        voluntario.RedefinirSenha = true;
        await _context.SaveChangesAsync();

        return (true, "Usuário pode acessar a aplicação e redefinir a senha.");
    }

    //public async Task<RespostaRecuperarSenhaDto> RecuperarSenhaPorTelefone(RecuperarSenhaDto dto)
    //{
    //    // Buscar usuário pelo nome e telefone
    //    var usuario = await _userManager.FindByNameAsync(dto.NomeUsuario);

    //    if (usuario == null || usuario.PhoneNumber != dto.Telefone)
    //    {
    //        return new RespostaRecuperarSenhaDto
    //        {
    //            Sucesso = false,
    //            Mensagem = "Usuário ou telefone inválidos.",
    //        };
    //    }

    //    var codigoVerificacao = GerarCodigoVerificacao(); // temporaário

    //    // Enviar código para WhatsApp ou SMS
    //    await EnviarCodigoPorWhatsAppOuSMS(usuario.PhoneNumber, codigoVerificacao);

    //    // TODO: Criar tabela
    //    var recuperacao = new RecuperacaoSenha
    //    {
    //        UsuarioId = usuario.Id,
    //        Codigo = codigoVerificacao,
    //        Expiracao = DateTime.UtcNow.AddMinutes(5), // 5 min
    //    };

    //    _context.RecuperacoesSenha.Add(recuperacao);
    //    await _context.SaveChangesAsync();

    //    await _sendGridClient.EnviarSmsAsync(dto.Telefone, $"Seu código de recuperação é: {codigoTemporario}"); ;

    //    return new RecuperarSenhaRespostaDto
    //    {
    //        Sucesso = true,
    //        Mensagem = "Código enviado com sucesso.",
    //        CodigoTemporario = codigoTemporario
    //    };

    //}

    #region MÉTODOS PRIVADOS

    private bool VerificarUsuarioNoBanco(string nomeUsuario, string telefone)
    {
        // Simulação de busca no banco de dados
        return nomeUsuario == "UserPadrao" && telefone == "15999999999";
    }

    #endregion
}
