using AAPS.Api.Dtos.Acompanhamento;
using AAPS.Api.Models;
using AAPS.Api.Responses;
using AAPS.Api.Services.Acompanhamentos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AcompanhamentoController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IAcompanhamentoService _acompanhamentoService;

    public AcompanhamentoController(IAcompanhamentoService acompanhamentoService)
    {
        _acompanhamentoService = acompanhamentoService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarAcompanhamento([FromBody] CriarAcompanhamentoDto acompanhamentoDto)
    {
        var erros = await _acompanhamentoService.ValidarCriacaoAcompanhamento(acompanhamentoDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar acompanhamento!"));
        }

        var acompanhamento = await _acompanhamentoService.CriarAcompanhamento(acompanhamentoDto);

        if (acompanhamento is null)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { $"Erro ao criar acompanhamento!" }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(acompanhamento, "Acompanhamento criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Acompanhamento>>> ObterAcompanhamentos()
    {
        var acompanhamentos = await _acompanhamentoService.ObterAcompanhamentos();

        if (acompanhamentos is null || !acompanhamentos.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Não há registros acompanhamentos para este animal." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(acompanhamentos));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterAcompanhamentoPorId(int id)
    {
        var acompanhamento = await _acompanhamentoService.ObterAcompanhamentoPorId(id);

        if (acompanhamento is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Acompanhamento não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(acompanhamento));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirAcompanhamento(int id)
    {
        var acompanhamento = await _acompanhamentoService.ExcluirAcompanhamento(id);

        if (!acompanhamento)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Acompanhamento não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(null, "Acompanhamento excluído com sucesso!"));
    }
}