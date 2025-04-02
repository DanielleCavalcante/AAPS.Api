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
    public int DoadorId { get; set; }
    public DisponibilidadeEnum Disponibilidade { get; set; } = DisponibilidadeEnum.Disponivel;

    // Relacionamentos
    public Doador Doador { get; set; }
    public ICollection<AnimalEvento> AnimalEvento { get; set; }
    public ICollection<Adocao> Adocoes { get; set; }
}