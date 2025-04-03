using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.PontoAdocao;

public class PontoAdocaoDto
{
    public int Id { get; set; }
    public required string NomeFantasia { get; set; }
    public required string Responsavel { get; set; }
    public required string Cnpj { get; set; }
    public required StatusEnum Status { get; set; }
    public required string Logradouro { get; set; }
    public required int Numero { get; set; }
    public string? Complemento { get; set; }
    public required string Bairro { get; set; }
    public required string Uf { get; set; }
    public required string Cidade { get; set; }
    public required string Cep { get; set; }
}