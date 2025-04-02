using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Evento
{
    public class AtualizarEventoDto
    {
        public string? Descricao { get; set; }
        public StatusEnum? Status { get; set; }
    }
}