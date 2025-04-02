using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Evento;

public class EventoDto
{
    public int Id { get; set; }
    public required string Descricao { get; set; }
    public required StatusEnum Status { get; set; }
}