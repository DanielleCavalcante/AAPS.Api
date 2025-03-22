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
    public bool Status { get; set; } = false; // ver se deixa booleano ou enum
    public int DoadorId { get; set; }

    // Relacionamentos
    public Doador Doador { get; set; } = new Doador();
    public ICollection<AnimalEvento> AnimalEvento { get; set; } = new List<AnimalEvento>(); // acompanhamento
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
}
