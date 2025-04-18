using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Adotante
{
    public class AtualizarAdotanteDto
    {
        public string? Nome { get; set; }
        public string? Rg { get; set; }
        public string? Cpf { get; set; }
        public StatusEnum? Status { get; set; }

        public List<string?> Telefones { get; set; }

        public string? LocalTrabalho { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public BloqueioEnum? Bloqueio { get; set; }
        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Cep { get; set; }
        public string? SituacaoEndereco { get; set; }
    }
}