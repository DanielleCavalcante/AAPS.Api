using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.PontosAdocao
{
    public class FiltroPontoAdocaoDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
    }
}