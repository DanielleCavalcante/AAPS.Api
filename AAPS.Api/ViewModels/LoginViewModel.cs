using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Nome de Usuário é obrigatório")]
    [StringLength(20, ErrorMessage = "O nome de usuário deve ter no máximo 20 caracteres.")]
    //[EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    public string NomeUsuario { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(20, ErrorMessage = "A senha deve ter entre 6 e 20 caracteres.",  MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Senha { get; set; }
}
