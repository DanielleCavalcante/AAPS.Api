using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Rg { get; set; }
    public string? Cpf { get; set; }
    public TipoPessoaEnum Tipo {  get; set; } 
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;

    public string? Celular { get; set; }
    public string? Contato { get; set; }
    public string? ResponsavelContato { get; set; }

    public string? Logradouro { get; set; }
    public int? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Uf { get; set; }
    public string? Cidade { get; set; }
    public string? Cep { get; set; }

    public string? SituacaoEndereco { get; set; }

    // Relacionamentos
    public Adotante? Adotante { get; set; }
    //public PontoAdocao? PontoAdocao { get; set; }
    public Voluntario? Voluntario { get; set; }
    //public Endereco? Endereco { get; set; }
    //public ICollection<Telefone>? Telefones { get; set; } = new List<Telefone>();
    public ICollection<Animal>? Animais { get; set; }
}