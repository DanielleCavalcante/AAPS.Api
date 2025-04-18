using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Voluntario
{
    public class AtualizarVoluntarioDto
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public StatusEnum? Status { get; set; }
        public string? UserName { get; set; } // ver se precisa
        public string? Email { get; set; } //ver se precisa
        public string? PhoneNumber { get; set; }
        public string? Acesso { get; set; }
    }
}