namespace AAPS.Api.Dtos.Animais;

public class AnimalDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Especie { get; set; }
    public string Raca { get; set; }
    public string Pelagem { get; set; }
    public string Sexo { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool Status { get; set; }
    public int DoadorId { get; set; }
}