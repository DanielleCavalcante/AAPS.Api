using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AAPS.Api.Models;

public class Doador
{
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public string Rg { get; set; }
    [Required]
    public string Cpf { get; set; }
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

    // Relacionamentos - vários animais
    public ICollection<Animal> Animais { get; set; }
    public ICollection<Telefone> Telefones { get; set; }
}
