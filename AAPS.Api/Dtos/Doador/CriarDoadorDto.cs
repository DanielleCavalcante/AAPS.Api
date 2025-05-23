using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Doador
{
    public class CriarDoadorDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Rg { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;

        //public List<string> Telefones { get; set; } = new List<string>();

        public string Celular { get; set; } = string.Empty;
        public string Contato { get; set; } = string.Empty;
        public string ResponsavelContato { get; set; } = string.Empty;

        public string Logradouro { get; set; } = string.Empty;
        public int Numero { get; set; } = 0;
        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
    }
}