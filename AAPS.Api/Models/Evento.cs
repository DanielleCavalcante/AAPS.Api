namespace AAPS.Api.Models;

public class Evento
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;

    public ICollection<AnimalEvento> AnimalEvento { get; set; }
}