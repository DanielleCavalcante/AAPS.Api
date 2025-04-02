using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Adotante
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string LocalTrabalho { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public int Cep { get; set; }
    public string SituacaoEndereco { get; set; }
    public BloqueioEnum Bloqueio { get; set; } = BloqueioEnum.Desbloquado;

    public ICollection<Telefone> Telefones { get; set; }
    public ICollection<Adocao> Adocoes { get; set; }
}