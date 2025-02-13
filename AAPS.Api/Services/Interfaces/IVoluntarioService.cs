using AAPS.Api.Dtos;

namespace AAPS.Api.Services.Interfaces;

public interface IVoluntarioService
{
    public Task RegistrarVoluntario(VoluntarioDto voluntarioDto);
}
