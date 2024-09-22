using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Evento
{
    public int Id { get; set; }
    public EventoEnum Code { get; set; }

    public ICollection<AnimalEvento> AnimalEventos { get; set; }
}