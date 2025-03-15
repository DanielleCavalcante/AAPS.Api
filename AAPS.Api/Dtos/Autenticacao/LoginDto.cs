using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Dtos.Autenticacao
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Nome de Usuário é obrigatório")]
        [StringLength(20, ErrorMessage = "O nome de usuário deve ter no máximo 20 caracteres.")]
        public required string NomeUsuario { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(20, ErrorMessage = "A senha deve ter entre 6 e 20 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public required string Senha { get; set; }
    }
}

