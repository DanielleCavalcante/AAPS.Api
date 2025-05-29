namespace AAPS.Api.Dtos.Adocao
{
    public class CancelarAdocaoDto
    {
        public DateTime DataAcompanhamento { get; set; } = DateTime.MinValue;
        public string Observacao { get; set; } = string.Empty;
        public int EventoId { get; set; } = 0;
    }
}