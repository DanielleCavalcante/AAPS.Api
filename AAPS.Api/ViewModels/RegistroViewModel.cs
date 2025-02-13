using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.ViewModels;

public class RegistroViewModel
{
    //[Required]
    //[EmailAddress]
    //public string Email { get; set; }

    [Required]
    public string NomeCompleto { get; set; }
    [Required]
    public string NomeUsuario { get; set; }
    [Required]
    public string Cpf { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Telefone { get; set; } // tabela identity


    //talvez tirar e criar tela de atribuiçao de perfil:
    [Required]
    public string Acesso { get; set; }

    //[Required] -- caso queiram selecionar ativo ou inativo na criacao
    //public string Status { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar senha")]
    [Compare("Senha", ErrorMessage = "Senhas não conferem")]
    public string ConfirmarSenha { get; set; }
}
