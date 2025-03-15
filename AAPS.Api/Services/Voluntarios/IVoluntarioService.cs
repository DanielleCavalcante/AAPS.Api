using AAPS.Api.Dtos.Voluntarios;

namespace AAPS.Api.Services.Voluntarios;

public interface IVoluntarioService
{
    Task<bool> RegistrarVoluntario(CriarVoluntarioDto voluntarioDto);
}
