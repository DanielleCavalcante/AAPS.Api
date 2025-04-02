namespace AAPS.Api.Dtos.Acompanhamento
{
    public class AcompanhamentoDto
    {
        public int Id { get; set; }
        public required DateTime Data { get; set; }
        public string? Observacao { get; set; }
        public required int AnimalId { get; set; }
        public required int EventoId { get; set; }
    }
}
