using AAPS.Api.Context;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Services.Voluntarios;

public class VoluntarioService : IVoluntarioService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly AppDbContext _context;
    private readonly UserManager<Voluntario> _userManager;
    private readonly SignInManager<Voluntario> _signInManager;

    public VoluntarioService(AppDbContext context, UserManager<Voluntario> userManager, SignInManager<Voluntario> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
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

        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(voluntario, voluntarioDto.Acesso);
            await _signInManager.SignInAsync(voluntario, isPersistent: false); // não persistir cookie de login
        }

        return resultado.Succeeded;
    }
}