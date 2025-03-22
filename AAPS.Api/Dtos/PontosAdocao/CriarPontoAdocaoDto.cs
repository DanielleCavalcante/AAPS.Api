namespace AAPS.Api.Dtos.PontosAdocao
{
    public class CriarPontoAdocaoDto
    {
        public required string NomeFantasia { get; set; } = string.Empty;
        public required string Responsavel { get; set; } = string.Empty;
        public required string Cnpj { get; set; } = string.Empty;
        public required string Logradouro { get; set; } = string.Empty;
        public required int Numero { get; set; } = 0;
        public required string Complemento { get; set; } = string.Empty;
        public required string Bairro { get; set; } = string.Empty;
        public required string Uf { get; set; } = string.Empty;
        public required string Cidade { get; set; } = string.Empty;
        public required int Cep { get; set; } = 0;
    }
}