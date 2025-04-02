using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Doador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public int Cep { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;

    public ICollection<Animal> Animais { get; set; }
    public ICollection<Telefone> Telefones { get; set; }
}