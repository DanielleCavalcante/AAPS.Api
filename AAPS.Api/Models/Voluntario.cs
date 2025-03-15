using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Models;

public class Voluntario : IdentityUser<int>
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public StatusEnum Status { get; set; }

    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
}
