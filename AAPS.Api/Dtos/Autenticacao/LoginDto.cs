using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Dtos.Autenticacao
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Nome de Usuário é obrigatório")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]
        [DataType(DataType.Password)]
        public required string Senha { get; set; }
    }
}