using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.PontoAdocao
{
    public class FiltroPontoAdocaoDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
    }
}