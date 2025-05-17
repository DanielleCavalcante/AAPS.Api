using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Adotante;

public class AdotanteDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Rg { get; set; }
    public required string Cpf { get; set; }
    public required StatusEnum Status { get; set; }

    //public required List<string> Telefones { get; set; }
    public required string Contato1 { get; set; }
    public required string Contato2 { get; set; }

    public required string LocalTrabalho { get; set; }
    public required string Facebook { get; set; }
    public required string Instagram { get; set; }
    public required BloqueioEnum Bloqueio { get; set; }

    public required string Logradouro { get; set; }
    public required int Numero { get; set; }
    public string? Complemento { get; set; }
    public required string Bairro { get; set; }
    public required string Uf { get; set; }
    public required string Cidade { get; set; }
    public required string Cep { get; set; }
    public required string SituacaoEndereco { get; set; }
}