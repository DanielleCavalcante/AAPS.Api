using AAPS.Api.Dtos.Adocao;
using AAPS.Api.Responses;
using AAPS.Api.Services;
using AAPS.Api.Services.Adocoes;
using AAPS.Api.Services.Adotantes;
using AAPS.Api.Services.TermoAdocao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TermoAdocaoController : Controlle
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly ITermoAdocaoService _termoAdocaoService;
    private readonly IAdotanteService _adotanteService;
    private readonly IAdocaoService _adocaoService;
    private readonly EmailService _emailService;

    public TermoAdocaoController(ITermoAdocaoService termoAdocao, IAdotanteService adotanteService, EmailService emailService, IAdocaoService adocaoService)
    {
        _termoAdocaoService = termoAdocao;
        _adotanteService = adotanteService;
        _emailService = emailService;
        _adocaoService = adocaoService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> GerarPdf([FromBody] int adocaoId)
    {
        var adocao = await _adocaoService.ObterAdocaoPorId(adocaoId);

        try
        {
            var pdfBytes = await _termoAdocaoService.GerarPdf(adocaoId);
            return File(pdfBytes, "application/pdf", $"TermoAdocao_{adocao.NomeAdotante}.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { $"Erro ao gerar o PDF: {ex.Message}" }));
        }
    }

    [HttpPost]
    public async Task<IActionResult> EnviarTermoAdocao([FromBody] EnviarTermoDto dto)
    {
        var adotante = await _adotanteService.ObterAdotantePorId(dto.AdotanteId);

        var adocao = await _adocaoService.ObterAdocaoPorId(dto.AdocaoId);

        if (adotante == null)
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Adotante não encontrado." }));

        var assunto = "Termo de Adoção - Associação Anjos e Protetores de Sorocaba";

        var corpoEmail = $@"
<p>Olá {adotante.Nome},</p>

<p>É com muita alegria que enviamos em anexo o Termo de Adoção referente ao animal escolhido por você em nosso evento de adoção.</p>

<p>
<strong>Número do Termo:</strong> {dto.AdocaoId}<br/>
<strong>Data da Adoção:</strong> {adocao.Data:dd/MM/yyyy}<br/>
</p>

<p>Agradecemos imensamente por proporcionar uma nova chance e muito amor ao nosso(a) amigo(a) de quatro patas!</p>

<p>Em caso de dúvidas ou necessidade de mais informações, fique à vontade para entrar em contato conosco.</p>

<br/>
<p>Atenciosamente,<br/><br/>
Associação Anjos e Protetores de Sorocaba</p>
";

        await _emailService.EnviarEmailComAnexoAsync(
            adotante.Email,
            assunto,
            corpoEmail,
            dto.AdocaoId
        );

        return Ok(ApiResponse<object>.SucessoResponse("Termo de Adoção enviado com sucesso!"));
    }
}
