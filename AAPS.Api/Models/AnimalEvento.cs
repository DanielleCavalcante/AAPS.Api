using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace AAPS.Api.Models;

public class AnimalEvento
{
    public int Id { get; set; }
    public DateTime DataAcompanhamento { get; set; } = DateTime.MinValue;
    public string Observacao { get; set; } = string.Empty;
    public int AnimalId { get; set; } = 0;
    public int EventoId { get; set; } = 0;

    // Relacionamentos
    public Animal Animal { get; set; }
    public Evento Evento { get; set; }
}
