namespace AAPS.Api.Dtos.Adotantes
{
    public class CriarAdotanteDto
    {
        public required string Nome { get; set; } = string.Empty;
        public required string Rg { get; set; } = string.Empty;
        public required string Cpf { get; set; } = string.Empty;
        public required string LocalTrabalho { get; set; } = string.Empty;
        public required bool Status { get; set; } = true;
        public required string Facebook { get; set; } = string.Empty;
        public required string Instagram { get; set; } = string.Empty;
        public required string Logradouro { get; set; } = string.Empty;
        public required int Numero { get; set; } = 0;
        public required string Complemento { get; set; } = string.Empty;
        public required string Bairro { get; set; } = string.Empty;
        public required string Uf { get; set; } = string.Empty;
        public required string Cidade { get; set; } = string.Empty;
        public required int Cep { get; set; } = 0;
        public required string SituacaoEndereco { get; set; } = string.Empty;
    }
}
