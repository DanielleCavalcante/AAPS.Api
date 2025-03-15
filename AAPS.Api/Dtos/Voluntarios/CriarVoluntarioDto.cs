using AAPS.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Dtos.Voluntarios
{
    public class CriarVoluntarioDto
    {
        public required string NomeCompleto { get; set; }
        public required string NomeUsuario { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public required StatusEnum Status { get; set; }
        public required string Acesso { get; set; }
        public required string Senha { get; set; }
        public string? ConfirmarSenha { get; set; }
    }
}
