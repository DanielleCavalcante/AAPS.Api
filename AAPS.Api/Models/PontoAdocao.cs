using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class PontoAdocao
{
    public int Id { get; set; }
    public string NomeFantasia { get; set; } = string.Empty;
    public string Responsavel { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public int Cep { get; set; } = 0;

    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
    public ICollection<Telefone> Telefones { get; set; } = new List<Telefone>();
}
