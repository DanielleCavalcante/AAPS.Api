namespace AAPS.Api.Dtos.PontosAdocao
{
    public class AtualizaPontoAdocaoDto
    {
        public string? NomeFantasia { get; set; }
        public string? Responsavel { get; set; }
        public string? Cnpj { get; set; }
        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public int? Cep { get; set; }
    }
}
