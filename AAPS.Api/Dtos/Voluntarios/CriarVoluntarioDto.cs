using AAPS.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Dtos.Voluntarios
{
    public class CriarVoluntarioDto
    {
        public required string NomeCompleto { get; set; } = string.Empty;
        public required string NomeUsuario { get; set; } = string.Empty;
        public required string Cpf { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Telefone { get; set; } = string.Empty;
        public required StatusEnum Status { get; set; } = StatusEnum.Ativo;
        public required string Acesso { get; set; } = string.Empty;
        public required string Senha { get; set; } = string.Empty;
        public string? ConfirmarSenha { get; set; } = string.Empty;
    }
}
