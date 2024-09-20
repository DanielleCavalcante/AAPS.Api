using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace AAPS.Api.Models;

public class AnimalEvento
{
    public int Id { get; set; }
    [Required]
    public DateTime DataAcompanhamento { get; set; }
    public string? Observacao { get; set; }
    public int AnimalId { get; set; }
    public int EventoId { get; set; }

    // Relacionamentos
    public Animal Animal { get; set; }
    public Evento Evento { get; set; }
}
