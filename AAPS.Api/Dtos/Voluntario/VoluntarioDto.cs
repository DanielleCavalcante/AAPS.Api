using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntario;

public class VoluntarioDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
    public required StatusEnum Status { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Acesso { get; set; }
}