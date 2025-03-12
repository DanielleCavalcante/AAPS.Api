using AAPS.Api.Dtos.Voluntarios;

namespace AAPS.Api.Services.Interfaces;

public interface IVoluntarioService
{
    public Task RegistrarVoluntario(VoluntarioDto voluntarioDto);
}
