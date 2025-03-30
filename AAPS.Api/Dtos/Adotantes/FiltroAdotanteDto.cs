using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Adotantes
{
    public class FiltroAdotanteDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
    }
}
