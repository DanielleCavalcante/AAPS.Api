namespace AAPS.Api.Dtos.Autenticacao
{
    public class TokenDto
    {
        public required string Token { get; set; }
        public DateTime Expiracao { get; set; }
    }
}
