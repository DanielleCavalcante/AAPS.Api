using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Doador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public int Cep { get; set; } = 0;
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;

    public ICollection<Animal> Animais { get; set; }
    public ICollection<Telefone> Telefones { get; set; }
}
