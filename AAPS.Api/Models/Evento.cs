using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Evento
{
    public int Id { get; set; }
    public EventoEnum Code { get; set; }
    public int AnimalEventoId { get; set; }

    public AnimalEvento AnimalEvento { get; set; }
    public ICollection<AnimalEvento> Acompanhamentos { get; set; }
}
