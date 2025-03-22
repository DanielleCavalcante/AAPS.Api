using AAPS.Api.Dtos.Voluntarios;

namespace AAPS.Api.Dtos.Autenticacao
{
    public class TokenDto
    {
        public required string Token { get; set; }
        public DateTime Expiracao { get; set; }
        public VoluntarioResponseDto Voluntario { get; set; } = new VoluntarioResponseDto();
    }
}