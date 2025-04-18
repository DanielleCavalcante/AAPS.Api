using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntario
{
    public class CriarVoluntarioDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Acesso { get; set; } = string.Empty;
        //public string Senha { get; set; } = string.Empty;
        //public string? ConfirmarSenha { get; set; } = string.Empty;
    }
}