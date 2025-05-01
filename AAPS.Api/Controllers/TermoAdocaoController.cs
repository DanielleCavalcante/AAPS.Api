using AAPS.Api.Dtos.TermoAdocao;
using AAPS.Api.Responses;
using AAPS.Api.Services.TermoAdocao;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TermoAdocaoController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly ITermoAdocaoService _termoAdocaoService;

    public TermoAdocaoController(ITermoAdocaoService termoAdocao)
    {
        _termoAdocaoService = termoAdocao;
    }

    #endregion

    [HttpPost]
    public IActionResult GerarPdf([FromBody] TermoAdocaoDto dto)
    {
        if (dto == null)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Os dados fornecidos são inválidos." }));
        }

        try
        {
            var pdfBytes = _termoAdocaoService.GerarPdf(dto);
            return File(pdfBytes, "application/pdf", $"TermoAdocao_{dto.Nome}.pdf");

            //var memoryStream = _termoAdocaoService.GerarPdf(dto);
            //return File(memoryStream.ToArray(), "application/pdf", $"TermoAdocao_{dto.Nome}.pdf");

            //string filePath = $"TermoAdocao_{dto.Nome}.pdf";
            //return Ok(ApiResponse<object>.SucessoResponse(new { FilePath = filePath }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { $"Erro ao gerar o PDF: {ex.Message}" }));
        }
    }
}
