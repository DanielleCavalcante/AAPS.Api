using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntarios
{
    public class VoluntarioResponseDto
    {
        public int Id { get; set; }
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;
        public string Acesso { get; set; } = string.Empty;
    }
}
