using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos;

public class VoluntarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public StatusEnum Status { get; set; }
    public bool RedefinirSenha { get; set; }
    public string IdentityUserId { get; set; }
    public string IdentityRoleId { get; set; }
}
