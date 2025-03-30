using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AAPS.Api.Services.Voluntarios;

public class VoluntarioService : IVoluntarioService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly AppDbContext _context;
    private readonly EmailService _emailService;
    private readonly UserManager<Voluntario> _userManager;
    private readonly SignInManager<Voluntario> _signInManager;

    public VoluntarioService(AppDbContext context, UserManager<Voluntario> userManager, SignInManager<Voluntario> signInManager, EmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    #endregion

    public async Task<bool> RegistrarVoluntario(CriarVoluntarioDto voluntarioDto)
    {
        var voluntario = new Voluntario
        {
            UserName = voluntarioDto.UserName,
            Email = voluntarioDto.Email,
            PhoneNumber = voluntarioDto.Telefone,
            Nome = voluntarioDto.Nome,
            Cpf = voluntarioDto.Cpf,
            Status = voluntarioDto.Status,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var resultado = await _userManager.CreateAsync(voluntario, voluntarioDto.Senha);

        if (!resultado.Succeeded)
        {
            return false;
        }

        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(voluntario, voluntarioDto.Acesso);
            await _signInManager.SignInAsync(voluntario, isPersistent: false); // não persistir cookie de login
        }

        return resultado.Succeeded;
    }

    public async Task<bool> RedefinirSenha(int voluntarioId)
    {
        var voluntario = await BuscarVoluntarioPorId(voluntarioId);
        if (voluntario == null)
            return false;

        string novaSenha = "Aaps@123";

        var removeResult = await _userManager.RemovePasswordAsync(voluntario);
        if (!removeResult.Succeeded)
            return false;

        var addResult = await _userManager.AddPasswordAsync(voluntario, novaSenha);
        return addResult.Succeeded;
    }

    public async Task<Voluntario?> ObterVoluntarioPorId(int id) //TODO: usar DTO
    {
        var voluntario = await BuscarVoluntarioPorId(id);

        if (voluntario == null)
        {
            return null;
        }

        return new Voluntario
        {
            Id = voluntario.Id,
            Nome = voluntario.Nome,
            UserName = voluntario.UserName,
            Email = voluntario.Email,
            PhoneNumber = voluntario.PhoneNumber,
            Cpf = voluntario.Cpf,
            Status = voluntario.Status,
            SecurityStamp = voluntario.SecurityStamp,
        };
    }

    public async Task<Voluntario?> ObterVoluntarioPorUserName(string username)
    {
        return await _context.Voluntarios.FirstOrDefaultAsync(v => v.UserName == username); //TODO: usar DTO
    }

    public async Task<Voluntario> BuscarVoluntarioPorUsernameETelefoneAsync(string username, string telefone) //TODO: usar DTO
    {
        var voluntario = await _context.Voluntarios
            .FirstOrDefaultAsync(v => v.UserName == username && v.PhoneNumber == telefone);

        if (voluntario == null)
        {
            return null;
        }

        return voluntario;
    }

    public async Task<List<Voluntario>> ObterAdministradoresAsync() //TODO: usar DTO
    {
        var admins = await _userManager.GetUsersInRoleAsync("Admin");

        return await _context.Voluntarios
            .Where(v => admins.Select(a => a.Id).Contains(v.Id))
            .ToListAsync();
    }

    public async Task<List<string>> ValidarCriacaoVoluntario(CriarVoluntarioDto voluntarioDto)
    {
        var erros = new List<string>();

        if (string.IsNullOrEmpty(voluntarioDto.Nome))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.UserName))
            erros.Add("O campo 'Nome de Usuário' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Cpf))
            erros.Add("O campo 'CPF' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Email))
            erros.Add("O campo 'Email' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Telefone))
            erros.Add("O campo 'Telefone' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Status.ToString()))
            erros.Add("O campo 'Status' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Acesso))
            erros.Add("O campo 'Acesso' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Senha))
            erros.Add("O campo 'Senha' é obrigatório!");
        if (voluntarioDto.Senha != voluntarioDto.ConfirmarSenha)
            erros.Add("As senhas não conferem!");

        if (!Regex.IsMatch(voluntarioDto.Senha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,}$"))
        {
            erros.Add("A senha deve ter pelo menos 8 caracteres, incluindo 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caractere especial.");
        }

        var usuarioExistente = await ObterVoluntarioPorUserName(voluntarioDto.UserName);
        if (usuarioExistente != null)
        {
            erros.Add("O nome de usuário já está em uso.");
        }

        var voluntarioExistente = await _userManager.Users
            .Where(v =>
                v.Nome == voluntarioDto.Nome ||
                v.Cpf == voluntarioDto.Cpf ||
                v.Email == voluntarioDto.Email ||
                v.PhoneNumber == voluntarioDto.Telefone
            )
            .FirstOrDefaultAsync();

        if (voluntarioExistente != null)
        {
            erros.Add($"Voluntário já cadastrado. Código {voluntarioExistente.Id}");
        }

        return erros;
    }

    #region MÉTODOS PRIVADOS

    private async Task<Voluntario?> BuscarVoluntarioPorId(int id)
    {
        return await _userManager.Users.FirstOrDefaultAsync(v => v.Id == id);
    }

    #endregion
}