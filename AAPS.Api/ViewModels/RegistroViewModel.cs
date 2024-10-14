using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.ViewModels;

public class RegistroViewModel
{
    //[Required]
    //[EmailAddress]
    //public string Email { get; set; }

    [Required]
    public string NomeUsuario { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirma senha")]
    [Compare("Senha", ErrorMessage = "Senhas não conferem")]
    public string ConfirmarSenha { get; set; }
}
