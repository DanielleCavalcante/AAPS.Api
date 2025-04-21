using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Animal
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Especie { get; set; }
    public string Raca { get; set; }
    public string Pelagem { get; set; }
    public string Sexo { get; set; }
    public DateTime? DataNascimento { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;
    public DisponibilidadeEnum Disponibilidade { get; set; } = DisponibilidadeEnum.Disponivel;

    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
    public ICollection<Acompanhamento>? Acompanhamentos { get; set; } = new List<Acompanhamento>();
    public ICollection<Adocao>? Adocoes { get; set; } = new List<Adocao>();
}