namespace AAPS.Api.Models;

public class AnimalEvento
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.MinValue;
    public string Observacao { get; set; } = string.Empty;
    public int AnimalId { get; set; }
    public int EventoId { get; set; }

    public Animal Animal { get; set; } = new Animal();
    public Evento Evento { get; set; } = new Evento();
}
