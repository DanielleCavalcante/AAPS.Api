using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Models;

public class Voluntario : IdentityUser<int>
{
    public string Nome{ get; set; }
    public string Cpf { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;

    public ICollection<Adocao> Adocoes { get; set; }
}