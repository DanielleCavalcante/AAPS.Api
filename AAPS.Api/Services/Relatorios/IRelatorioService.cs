using AAPS.Api.Dtos.Relatorio;
using System.Data;

namespace AAPS.Api.Services.Relatorios
{
    public interface IRelatorioService
    {
        DataTable[] ObterDadosRelatorio(DateTime dataInicio, DateTime dataFim);
        byte[] GerarRelatorioPdf(DateTime dataInicio, DateTime dataFim);
    }
}