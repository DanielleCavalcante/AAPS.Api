using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.PontoAdocao
{
    public class CriarPontoAdocaoDto
    {
        public string NomeFantasia { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public int Numero { get; set; } = 0;
        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public int Cep { get; set; } = 0;
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;
    }
}