namespace AAPS.Api.Dtos.Autenticacao
{
    public class TokenDto
    {
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
        public required string Role { get; set; }
    }
}