using System.ComponentModel.DataAnnotations;

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
    public bool Status { get; set; } = false;
    public int DoadorId { get; set; } = 0;

    // Relacionamentos
    public Doador Doador { get; set; }
    public ICollection<AnimalEvento> AnimalEventos { get; set; } = new List<AnimalEvento>();
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
}
