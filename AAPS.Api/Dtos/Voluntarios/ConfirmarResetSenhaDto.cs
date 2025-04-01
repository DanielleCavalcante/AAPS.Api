namespace AAPS.Api.Dtos.Voluntarios
{
    public class ConfirmarResetSenhaDto
    {
        public string UserName { get; set; }
        public string Telefone { get; set; }
        public string CodigoRecuperacao { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmarNovaSenha { get; set; }
    }
}
