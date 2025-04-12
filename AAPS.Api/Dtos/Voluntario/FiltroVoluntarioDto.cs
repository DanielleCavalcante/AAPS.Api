using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntario
{
    public class FiltroVoluntarioDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
    }
}