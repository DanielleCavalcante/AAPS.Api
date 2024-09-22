using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public string Cpf { get; set; }
    [Required]
    public string Senha { get; set; }
    [Required]
    public int Nivel { get; set; }
    [Required]
    public bool Status { get; set; }

    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; }
}
