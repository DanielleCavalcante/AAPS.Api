namespace AAPS.Api.Models;

public class AnimalEvento
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string? Observacao { get; set; }
    public int AnimalId { get; set; }
    public int EventoId { get; set; }

    public Animal Animal { get; set; }
    public Evento Evento { get; set; }
}