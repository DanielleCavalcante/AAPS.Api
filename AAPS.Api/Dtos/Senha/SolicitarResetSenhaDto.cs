namespace AAPS.Api.Dtos.Senha
{
    public class SolicitarResetSenhaDto
    {
        public required string UserName { get; set; }
        public required string Telefone { get; set; }
    }
}