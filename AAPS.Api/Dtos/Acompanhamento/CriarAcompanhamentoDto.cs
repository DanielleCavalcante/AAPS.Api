namespace AAPS.Api.Dtos.Acompanhamento
{
    public class CriarAcompanhamentoDto
    {
        public DateTime Data { get; set; } = DateTime.MinValue;
        public string Observacao { get; set; } = string.Empty;
        public int AnimalId { get; set; } = 0;
        public int EventoId { get; set; } = 0;
    }
}
