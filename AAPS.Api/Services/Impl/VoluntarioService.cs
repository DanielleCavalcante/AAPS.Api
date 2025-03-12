using AAPS.Api.Context;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;

namespace AAPS.Api.Services.Impl;

public class VoluntarioService : IVoluntarioService
{
    private readonly AppDbContext _context;

    public VoluntarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarVoluntario(VoluntarioDto voluntarioDto)
    {
        try
        {
            var voluntario = new Voluntario
            {
                Nome = voluntarioDto.Nome,
                Cpf = voluntarioDto.Cpf,
                Status = voluntarioDto.Status,
                RedefinirSenha = voluntarioDto.RedefinirSenha,
                IdentityUserId = voluntarioDto.IdentityUserId,
                IdentityRoleId = voluntarioDto.IdentityRoleId,
            };

            _context.Voluntarios.Add(voluntario);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
