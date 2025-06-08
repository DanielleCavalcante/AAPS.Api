using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Relatorio
{
    public class FiltroRelatorioDto
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public TipoRelatorio? Tipo { get; set; }
    }
}