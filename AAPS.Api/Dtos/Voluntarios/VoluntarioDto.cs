using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntarios;

public class VoluntarioDto
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string Cpf { get; set; }
    public StatusEnum Status { get; set; }
    public bool RedefinirSenha { get; set; }
}
