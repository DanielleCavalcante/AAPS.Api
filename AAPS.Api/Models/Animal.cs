using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Animal
{
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public string Especie { get; set; }
    [Required]
    public string Raca { get; set; }
    [Required]
    public string Pelagem { get; set; }
    [Required]
    public string Sexo { get; set; }
    public DateTime DataNascimento { get; set; }
    [Required]
    public bool Status { get; set; }
    public int DoadorId { get; set; }

    // Relacionamentos
    public Doador Doador { get; set; }
    public ICollection<AnimalEvento> AnimalEventos { get; set; }
    public ICollection<Adocao> Adocoes { get; set; }
}
