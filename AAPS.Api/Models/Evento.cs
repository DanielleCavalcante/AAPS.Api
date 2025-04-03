using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Evento
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Ativo;

    public ICollection<Acompanhamento>? Acompanhamentos { get; set; }
}