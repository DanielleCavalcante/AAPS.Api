using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Voluntario : IdentityUser<int>
{
    public string NomeCompleto { get; set; }
    public string Cpf { get; set; }
    public StatusEnum Status { get; set; } // ativo ou inativo

    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; }
}
