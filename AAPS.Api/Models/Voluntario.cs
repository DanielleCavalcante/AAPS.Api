using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Voluntario
{
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public string Cpf { get; set; }
    //[Required]
    //public string Senha { get; set; }
    //[Required]
    //public int Nivel { get; set; }
    [Required]
    public StatusEnum Status { get; set; } // ativo ou inativo

    [Required]
    public bool RedefinirSenha { get; set; }
    [Required]
    public string IdentityUserId { get; set; }
    [Required]
    public string IdentityRoleId { get; set; }

    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public IdentityRole IdentityRole { get; set; }
}
