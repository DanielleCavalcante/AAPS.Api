using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Adotante
{
    public class FiltroAdotanteDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
        public BloqueioEnum? Bloqueio { get; set; }
    }
}