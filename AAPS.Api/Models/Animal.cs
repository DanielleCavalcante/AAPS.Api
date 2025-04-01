using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Animal
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raca { get; set; } = string.Empty;
    public string Pelagem { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; } = DateTime.MinValue;
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;
    public int DoadorId { get; set; }
    public DisponibilidadeEnum Disponibilidade { get; set; } = DisponibilidadeEnum.Disponivel;

    // Relacionamentos
    public Doador Doador { get; set; }
    public ICollection<AnimalEvento> AnimalEvento { get; set; } = new List<AnimalEvento>(); // acompanhamento
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
}
