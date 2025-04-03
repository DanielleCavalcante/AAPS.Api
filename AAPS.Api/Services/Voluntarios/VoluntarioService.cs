using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
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

    public async Task<bool> CriarVoluntario(CriarVoluntarioDto voluntarioDto)
    {
        var pessoa = new Pessoa
        {
            Nome = voluntarioDto.Nome,
            Cpf = voluntarioDto.Cpf,
            Tipo = TipoPessoaEnum.Voluntario,
            Status = voluntarioDto.Status,
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        var voluntario = new Voluntario
        {
            UserName = voluntarioDto.UserName,
            Email = voluntarioDto.Email,
            PhoneNumber = voluntarioDto.PhoneNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
            PessoaId = pessoa.Id
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

    public async Task<VoluntarioDto?> ObterVoluntarioPorId(int id) //TODO: usar DTO
    {
        var voluntario = await BuscarVoluntarioPorId(id);

        var role = await _userManager.GetRolesAsync(voluntario);

        if (voluntario == null)
        {
            return null;
        }

        return new VoluntarioDto
        {
            Id = voluntario.Id,
            Nome = voluntario.Pessoa.Nome,
            UserName = voluntario.UserName,
            Email = voluntario.Email,
            PhoneNumber = voluntario.PhoneNumber,
            Cpf = voluntario.Pessoa.Cpf,
            Status = voluntario.Pessoa.Status,
            Acesso = role.FirstOrDefault()
            //SecurityStamp = voluntario.SecurityStamp,
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
        if (string.IsNullOrEmpty(voluntarioDto.Cpf))
            erros.Add("O campo 'CPF' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Status.ToString()))
            erros.Add("O campo 'Status' é obrigatório!");

        if (string.IsNullOrEmpty(voluntarioDto.UserName))
            erros.Add("O campo 'Nome de Usuário' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Email))
            erros.Add("O campo 'Email' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.PhoneNumber))
            erros.Add("O campo 'Telefone' é obrigatório!");
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
                v.Pessoa.Nome == voluntarioDto.Nome &&
                v.Pessoa.Cpf == voluntarioDto.Cpf &&
                v.Email == voluntarioDto.Email &&
                v.PhoneNumber == voluntarioDto.PhoneNumber
            )
            .FirstOrDefaultAsync();

        if (voluntarioExistente != null)
        {
            erros.Add($"Voluntário já cadastrado. Código {voluntarioExistente.Id}");
        }

        return erros;
    }

    //public async Task<bool> AlterarSenhaAsync(int voluntarioId, string novaSenha)
    //{
    //    var voluntario = await ObterVoluntarioPorId(voluntarioId);
    //    if (voluntario == null)
    //        return false;

    //    // Validação de senha (ex: complexidade mínima, etc.)
    //    if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
    //    {
    //        // Senha deve ter pelo menos 6 caracteres
    //        return false;
    //    }

    //    // Atualizando a senha (assumindo que a senha é salva de forma segura)
    //    voluntario.PasswordHash = novaSenha;  // A senha deve ser armazenada de forma segura, como um hash

    //    await _context.SaveChangesAsync();

    //    return true;
    //}

    #region MÉTODOS PRIVADOS

    private async Task<Voluntario?> BuscarVoluntarioPorId(int id)
    {
        return await _userManager.Users.FirstOrDefaultAsync(v => v.Id == id);
    }

    #endregion
}