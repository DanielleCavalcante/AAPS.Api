using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntarios;

public class VoluntarioDto
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;
}
