using Microsoft.EntityFrameworkCore.Update;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Adotante
{
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public string Rg { get; set; }
    [Required]
    public string Cpf { get; set; }
    public string  LocalTrabalho { get; set; }
    [Required]
    public bool Status { get; set; } 
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    [Required]
    public string Logradouro { get; set; }
    [Required]
    public int Numero { get; set; }
    public string Complemento { get; set; }
    [Required]
    public string Bairro { get; set; }
    [Required]
    public string Uf { get; set; }
    [Required]
    public string Cidade { get; set; }
    public int Cep { get; set; }
    [Required]
    public string SituacaoEndereco { get; set; }

    // Relacionamentos
    public ICollection<Telefone> Telefones { get; set; }
    public ICollection<Adocao> Adocoes { get; set; }
}
