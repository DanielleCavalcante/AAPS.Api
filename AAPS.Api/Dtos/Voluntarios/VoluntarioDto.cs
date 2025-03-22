using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntarios;

public class VoluntarioDto
{
    public int Id { get; set; }
    public required string Nome { get; set; } = string.Empty;
    public required string Cpf { get; set; } = string.Empty;
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;
    public required string UserName { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string Telefone { get; set; } = string.Empty;
    public string Acesso { get; set; } = string.Empty;
}