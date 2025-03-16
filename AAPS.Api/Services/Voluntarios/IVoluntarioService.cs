using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Services.Voluntarios;

public interface IVoluntarioService
{
    Task<bool> RegistrarVoluntario(CriarVoluntarioDto voluntarioDto);
    Task<bool> RedefinirSenha(int voluntarioId);
    Task<Voluntario> BuscarUsuarioPorUsernameETelefoneAsync(string username, string telefone);
    Task<List<Voluntario>> ObterAdministradoresAsync();
}
