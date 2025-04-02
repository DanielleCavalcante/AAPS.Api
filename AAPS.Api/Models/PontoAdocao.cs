using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class PontoAdocao
{
    public int Id { get; set; }
    public string NomeFantasia { get; set; }
    public string Responsavel { get; set; }
    public string Cnpj { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public int Cep { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;

    public ICollection<Adocao> Adocoes { get; set; }
    public ICollection<Telefone> Telefones { get; set; }
}