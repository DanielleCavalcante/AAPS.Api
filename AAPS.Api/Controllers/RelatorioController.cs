using AAPS.Api.Dtos.Relatorio;
using AAPS.Api.Responses;
using AAPS.Api.Services.Relatorios;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
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
    public IActionResult GerarRelatório([FromQuery] FiltroRelatorioDto filtro)
    {
        try
        {
            var dados = _relatorioService.ObterDadosRelatorio(filtro);

            using (var workbook = new XLWorkbook())
            {
                var caminhoLogo = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "aaps_logo.png");

                for (int i = 0; i < dados.Length; i++)
                {
                    var nomeAba = dados[i].TableName ?? $"Aba{i + 1}";
                    var planilha = workbook.Worksheets.Add(nomeAba);

                    if (System.IO.File.Exists(caminhoLogo))
                    {
                        planilha.Row(1).Height = 80;
                        planilha.Column(1).AdjustToContents();

                        var img = planilha.AddPicture(caminhoLogo)
                            .MoveTo(planilha.Cell("A1"))
                            .Scale(0.4); 
                    }

                    var tabela = planilha.Cell(2, 2).InsertTable(dados[i]);
                    tabela.Worksheet.Columns().AdjustToContents();
                }

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Relatório - AAPS.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { $"Erro ao gerar relatório: {ex.Message}" }));
        }
    }
}