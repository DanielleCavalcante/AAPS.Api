using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Adotante
{
    public class CriarAdotanteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Rg { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;
        public string LocalTrabalho { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public BloqueioEnum Bloqueio { get; set; } = BloqueioEnum.Desbloquado;
        public string Logradouro { get; set; } = string.Empty;
        public int Numero { get; set; } = 0;
        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string SituacaoEndereco { get; set; } = string.Empty;
    }
}