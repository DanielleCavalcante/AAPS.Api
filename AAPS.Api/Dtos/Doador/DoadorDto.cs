using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Doador;

public class DoadorDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Rg { get; set; }
    public required string Cpf { get; set; }
    public required string Logradouro { get; set; }
    public required int Numero { get; set; }
    public string? Complemento { get; set; }
    public required string Bairro { get; set; }
    public required string Uf { get; set; }
    public required string Cidade { get; set; }
    public required int Cep { get; set; }
    public required StatusEnum Status { get; set; }
}