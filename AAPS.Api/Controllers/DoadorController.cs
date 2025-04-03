using AAPS.Api.Dtos.Doador;
using AAPS.Api.Responses;
using AAPS.Api.Services.Doadores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DoadorController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IDoadorService _doadorService;

    public DoadorController(IDoadorService doadorService)
    {
        _doadorService = doadorService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarDoador([FromBody] CriarDoadorDto doadorDto)
    {
        var erros = await _doadorService.ValidarCriacaoDoador(doadorDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar doador!"));
        }

        var doador = await _doadorService.CriarDoador(doadorDto);

        if (doador is null)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { "Erro criar o doador." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(doador, $"Doador criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<DoadorDto>>> ObterDoadores([FromQuery] FiltroDoadorDto filtro)
    {
        var doadores = await _doadorService.ObterDoadores(filtro);

        if (doadores is null || !doadores.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum doador foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(doadores));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterDoadorPorId(int id)
    {
        var doador = await _doadorService.ObterDoadorPorId(id);

        if (doador is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Doador de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(doador));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<DoadorDto>>> ObterDoadoresAtivos()
    {
        var doadores = await _doadorService.ObterDoadoresAtivos();

        if (doadores is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum doador foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(doadores));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarDoador(int id, [FromBody] AtualizarDoadorDto doadorDto)
    {
        var erros = _doadorService.ValidarAtualizacaoDoador(doadorDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar doadors!"));
        }

        var doador = await _doadorService.AtualizarDoador(id, doadorDto);

        if (doador is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Doador de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Doador atualizado com sucesso!"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> ExcluirDoador(int id)
    {
        bool doador = await _doadorService.ExcluirDoador(id);

        if (!doador)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Doador de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Doador de id = {id} foi inativado com sucesso!"));
    }
}