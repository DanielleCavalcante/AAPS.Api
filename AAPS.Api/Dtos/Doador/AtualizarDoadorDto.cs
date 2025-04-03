using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Doador
{
    public class AtualizarDoadorDto
    {
        public string? Nome { get; set; }
        public string? Rg { get; set; }
        public string? Cpf { get; set; }
        public StatusEnum? Status { get; set; }
        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Cep { get; set; }
    }
}