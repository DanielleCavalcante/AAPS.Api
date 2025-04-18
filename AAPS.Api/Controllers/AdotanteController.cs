using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Responses;
using AAPS.Api.Services.Adotantes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdotanteController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IAdotanteService _adotanteService;

    public AdotanteController(IAdotanteService adotanteService)
    {
        _adotanteService = adotanteService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarAdotante([FromBody] CriarAdotanteDto adotanteDto)
    {
        var erros = await _adotanteService.ValidarCriacaoAdotante(adotanteDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar voluntário!"));
        }

        var adotante = await _adotanteService.CriarAdotante(adotanteDto);

        if (adotante is null)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar o adotante." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotante, "Adotante criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<AdotanteDto>>> ObterAdotantes([FromQuery] FiltroAdotanteDto filtro)
    {
        var adotantes = await _adotanteService.ObterAdotantes(filtro);

        if (adotantes is null || !adotantes.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum adotante foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotantes));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterAdotantePorId(int id)
    {
        var adotante = await _adotanteService.ObterAdotantePorId(id);

        if (adotante is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adotante de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotante));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<AdotanteDto>>> ObterAdotantesAtivos()
    {
        var adotantes = await _adotanteService.ObterAdotantesAtivos();

        if (adotantes is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum adotante foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotantes));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarAdotante(int id, [FromBody] AtualizarAdotanteDto adotanteDto)
    {
        var erros = _adotanteService.ValidarAtualizacaoAdotante(adotanteDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar adotante!"));
        }

        var adotante = await _adotanteService.AtualizarAdotante(id, adotanteDto);

        if (adotante is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adotante de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Adotante de id = {id} atualizado com sucesso!"));
    }

    //[HttpDelete("{id:int}")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> ExcluirAdotante(int id)
    {
        bool adotante = await _adotanteService.ExcluirAdotante(id);

        if (!adotante)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adotante de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Adotante de id = {id} foi inativado com sucesso!"));
    }

    //[HttpGet]
    //public async Task<ActionResult<IAsyncEnumerable<Adotante>>> ObterAdotantePorNome([FromQuery] string nome)
    //{
    //    var adotantes = await _adotanteService.ObterAdotantePorNome(nome);

    //    if (adotantes is null)
    //    {
    //        return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao obter adotantes." }));
    //    }

    //    return Ok(ApiResponse<object>.SucessoResponse(adotantes));
    //}
}