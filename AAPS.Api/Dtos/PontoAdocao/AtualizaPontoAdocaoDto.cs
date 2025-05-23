using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.PontoAdocao
{
    public class AtualizaPontoAdocaoDto
    {
        public string? NomeFantasia { get; set; }
        public string? Responsavel { get; set; }
        public string? Cnpj { get; set; }
        public StatusEnum? Status { get; set; }

        //public List<string?> Telefones { get; set; }

        public string? Celular { get; set; }
        public string? Contato { get; set; }
        public string? ResponsavelContato { get; set; }

        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Cep { get; set; }
    }
}