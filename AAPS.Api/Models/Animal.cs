using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Animal
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Especie { get; set; }
    public string Raca { get; set; }
    public string Pelagem { get; set; }
    [Required]
    public char Sexo { get; set; }
    public DateTime DataNascimento { get; set; }
    [Required]
    public int Status { get; set; }
    public int DoadorId { get; set; }

    // Relacionamentos
    public Doador Doador { get; set; }
    // TODO: tem que rever o relacionamento
    public AnimalEvento Acompanhamentos { get; set; }
    //public ICollection<AnimalEvento> Acompanhamento { get; set; }
    public ICollection<Adocao> Adocoes { get; set; }
}
