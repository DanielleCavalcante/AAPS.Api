using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.PontoAdocao
{
    public class CriarPontoAdocaoDto
    {
        public string NomeFantasia { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;

        public List<string> Telefones { get; set; } = new List<string>();

        public string Logradouro { get; set; } = string.Empty;
        public int Numero { get; set; } = 0;
        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
    }
}