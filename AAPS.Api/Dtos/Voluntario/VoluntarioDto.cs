using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntarios;

public class VoluntarioDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
    public required StatusEnum Status { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Telefone { get; set; }
    public required string Acesso { get; set; }
}