using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class PontoAdocao
{
    public int Id { get; set; }
    [Required]
    public string NomeFantasia { get; set; }
    [Required]
    public string Responsavel { get; set; }
    [Required]
    public string Cnpj { get; set; }
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
    [Required]
    public int Cep { get; set; }
    
    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; }
    public ICollection<Telefone> Telefones { get; set; }
}
