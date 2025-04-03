namespace AAPS.Api.Models;

public class Endereco
{
    public int Id { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public string Cep { get; set; }
    public string? SituacaoEndereco { get; set; }

    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
}