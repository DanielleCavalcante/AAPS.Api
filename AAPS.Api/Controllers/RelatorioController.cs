using AAPS.Api.Dtos.Relatorio;
using AAPS.Api.Responses;
using AAPS.Api.Services.Relatorios;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class RelatorioController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IRelatorioService _relatorioService;

    public RelatorioController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    #endregion

    [HttpGet]
    public IActionResult GerarRelatorio([FromQuery] FiltroRelatorioDto filtro)
    {
        try
        {
            //var partesInicio = filtro.DataInicio.Split('/');
            //int mesInicio = int.Parse(partesInicio[0]);
            //int anoInicio = int.Parse(partesInicio[1]);
            //var dataInicio = new DateTime(anoInicio, mesInicio, 1);

            //var partesFim = filtro.DataFim.Split('/');
            //int mesFim = int.Parse(partesFim[0]);
            //int anoFim = int.Parse(partesFim[1]);
            //int ultimoDiaFim = DateTime.DaysInMonth(anoFim, mesFim);
            //var dataFim = new DateTime(anoFim, mesFim, ultimoDiaFim);

            var dados = _relatorioService.ObterDadosRelatorio(filtro.DataInicio, filtro.DataFim);

            var dataInicioStr = filtro.DataInicio.ToString("MM/yyyy");
            var dataFimStr = filtro.DataFim.ToString("MM/yyyy");

            var nomeArquivo = $"Relatório - AAPS ({dataInicioStr} a {dataFimStr})";

            using (var workbook = new XLWorkbook())
            {
                var caminhoLogo = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "aaps_logo.png");

                var abaPrincipal = workbook.Worksheets.Add("Balanço Geral");

                if (System.IO.File.Exists(caminhoLogo))
                {
                    abaPrincipal.Row(1).Height = 80;
                    abaPrincipal.Column(1).AdjustToContents();

                    var img = abaPrincipal.AddPicture(caminhoLogo)
                        .MoveTo(abaPrincipal.Cell("A1"))
                        .Scale(0.4);
                }

                int linhaAtual = 3;

                for (int i = 0; i < dados.Length - 1; i++)
                {
                    var nomeTabela = dados[i].TableName ?? $"Tabela {i + 1}";

                    abaPrincipal.Cell(linhaAtual, 2).Value = nomeTabela;
                    abaPrincipal.Cell(linhaAtual, 2).Style.Font.Bold = true;
                    abaPrincipal.Cell(linhaAtual, 2).Style.Font.FontSize = 14;

                    linhaAtual += 2;

                    var tabela = abaPrincipal.Cell(linhaAtual, 2).InsertTable(dados[i]);
                    tabela.Worksheet.Columns().AdjustToContents();

                    linhaAtual += tabela.RowCount() + 3;
                }

                var abaBalanco = workbook.Worksheets.Add("Levantamento de Animais");

                if (System.IO.File.Exists(caminhoLogo))
                {
                    abaBalanco.Row(1).Height = 80;
                    abaBalanco.Column(1).AdjustToContents();

                    var img = abaBalanco.AddPicture(caminhoLogo)
                        .MoveTo(abaBalanco.Cell("A1"))
                        .Scale(0.4);
                }

                abaBalanco.Cell(2, 2).Value = "Animais adotados no período";
                abaBalanco.Cell(2, 2).Style.Font.Bold = true;
                abaBalanco.Cell(2, 2).Style.Font.FontSize = 14;

                var tabelaBalanco = abaBalanco.Cell(4, 2).InsertTable(dados[dados.Length - 1]);
                tabelaBalanco.Worksheet.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                $"{nomeArquivo}.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { $"Erro ao gerar relatório: {ex.Message}" }));
        }
    }

    [HttpGet]
    public IActionResult GerarRelatorioPdf([FromQuery] FiltroRelatorioDto filtro)
    {
        //var partesInicio = filtro.DataInicio.Split('/');
        //int mesInicio = int.Parse(partesInicio[0]);
        //int anoInicio = int.Parse(partesInicio[1]);
        //var dataInicio = new DateTime(anoInicio, mesInicio, 1);

        //var partesFim = filtro.DataFim.Split('/');
        //int mesFim = int.Parse(partesFim[0]);
        //int anoFim = int.Parse(partesFim[1]);
        //int ultimoDiaFim = DateTime.DaysInMonth(anoFim, mesFim);
        //var dataFim = new DateTime(anoFim, mesFim, ultimoDiaFim);

        if (filtro.DataInicio > filtro.DataFim)
        {
            return BadRequest("A data de início não pode ser maior que a data de fim.");
        }

        var dataInicioStr = filtro.DataInicio.ToString("MM/yyyy");
        var dataFimStr = filtro.DataFim.ToString("MM/yyyy");

        var pdfBytes = _relatorioService.GerarRelatorioPdf(filtro.DataInicio, filtro.DataFim);
        var fileName = $"Relatório - AAPS ({dataInicioStr} a {dataFimStr}).pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }
}