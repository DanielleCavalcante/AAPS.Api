namespace AAPS.Api.Dtos.Relatorio
{
    public class FiltroRelatorioDto
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string? Especie { get; set; }
        public string? FaixaEtaria { get; set; }
    }
}