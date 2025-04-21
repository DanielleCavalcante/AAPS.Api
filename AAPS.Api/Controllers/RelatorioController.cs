using AAPS.Api.Dtos.Relatorio;
using AAPS.Api.Responses;
using AAPS.Api.Services.Relatorios;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
    public IActionResult GerarRelatório([FromQuery] FiltroRelatorioDto filtro)
    {
        try
        {
            var dados = _relatorioService.ObterDadosRelatorio(filtro);

            var nomeMes = new DateTime(filtro.Ano, filtro.Mes, 1).ToString("MMMM", new CultureInfo("pt-BR"));
            var nomeArquivo = $"Relatório - AAPS ({nomeMes}/{filtro.Ano})".ToLower();

            using (var workbook = new XLWorkbook())
            {
                var caminhoLogo = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "aaps_logo.png");

                for (int i = 0; i < dados.Length - 2; i++)
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

                var abaBalanco = workbook.Worksheets.Add("Balanço Mensal");

                if (System.IO.File.Exists(caminhoLogo))
                {
                    abaBalanco.Row(1).Height = 80;
                    abaBalanco.Column(1).AdjustToContents();

                    var img = abaBalanco.AddPicture(caminhoLogo)
                        .MoveTo(abaBalanco.Cell("A1"))
                        .Scale(0.4);
                }

                var tabelaAdocoes = abaBalanco.Cell(2, 2).InsertTable(dados[dados.Length - 2]);
                tabelaAdocoes.Worksheet.Columns().AdjustToContents();

                // Calcula a linha onde começa a próxima tabela (após a adoções)
                int linhaInicioResgates = 2 + tabelaAdocoes.RowCount() + 3; // espaço entre as tabelas

                // Insere a tabela de Resgates
                var tabelaResgates = abaBalanco.Cell(linhaInicioResgates, 2).InsertTable(dados[dados.Length - 1]);
                tabelaResgates.Worksheet.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{nomeArquivo}.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { $"Erro ao gerar relatório: {ex.Message}" }));
        }
    }
}