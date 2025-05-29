namespace AAPS.Api.Dtos.Acompanhamento
{
    public class RetornaAcompanhamentoDto
    {
        public int Id { get; set; }
        public required DateTime Data { get; set; }
        public string? Observacao { get; set; }
        public required int AnimalId { get; set; }
        public required int EventoId { get; set; }
        public required string Descricao { get; set; }
    }
}