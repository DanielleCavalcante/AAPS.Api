using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Evento
{
    public class CriarEventoDto
    {
        public string Descricao { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;
    }
}