using Microsoft.EntityFrameworkCore.Update;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Adotante
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string  LocalTrabalho { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public string Facebook { get; set; } = string.Empty;
    public string Instagram { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public int Cep { get; set; } = 0;
    public string SituacaoEndereco { get; set; } = string.Empty;

    // Relacionamentos
    public ICollection<Telefone> Telefones { get; set; } = new List<Telefone>();
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
}
