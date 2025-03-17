using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            UserName = voluntarioDto.NomeUsuario,
            Email = voluntarioDto.Email,
            PhoneNumber = voluntarioDto.Telefone,
            NomeCompleto = voluntarioDto.NomeCompleto,
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
            NomeCompleto = voluntario.NomeCompleto,
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

    #region MÉTODOS PRIVADOS

    private async Task<Voluntario?> BuscarVoluntarioPorId(int id)
    {
        return await _userManager.Users.FirstOrDefaultAsync(v => v.Id == id);
    }

    #endregion
}