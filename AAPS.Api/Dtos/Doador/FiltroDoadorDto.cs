using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Doadores
{
    public class FiltroDoadorDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
    }
}