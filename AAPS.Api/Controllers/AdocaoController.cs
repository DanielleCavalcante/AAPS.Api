using AAPS.Api.Dtos.Adocao;
using AAPS.Api.Responses;
using AAPS.Api.Services.Adocoes;
using AAPS.Api.Services.Adotantes;
using AAPS.Api.Services.Animais;
using AAPS.Api.Services.PontosAdocao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdocaoController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IAdocaoService _adocaoService;
    private readonly IAdotanteService _adotanteService;
    private readonly IAnimalService _animalService;
    private readonly IVoluntarioService _voluntarioService;
    private readonly IPontoAdocaoService _pontoAdocaoService;

    public AdocaoController(IAdocaoService adocaoService, IAdotanteService adotanteService, IAnimalService animalService, IVoluntarioService voluntarioService, IPontoAdocaoService pontoAdocaoService)
    {
        _adocaoService = adocaoService;
        _adotanteService = adotanteService;
        _animalService = animalService;
        _voluntarioService = voluntarioService;
        _pontoAdocaoService = pontoAdocaoService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarAdocao([FromBody] CriarAdocaoDto adocaoDto)
    {
        var erros = await _adocaoService.ValidarCriacaoAdocao(adocaoDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar adoção!"));
        }

        var adocao = await _adocaoService.CriarAdocao(adocaoDto);
        if (adocao is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar adoção" }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adocao, "Adoção criada com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<AdocaoDto>>> ObterAdocoes([FromQuery] FiltroAdocaoDto filtro)
    {
        var adocoes = await _adocaoService.ObterAdocoes(filtro);

        if (adocoes is null || !adocoes.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhuma adoção foi encontrada." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adocoes));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterAdocaoPorId(int id)
    {
        var adocao = await _adocaoService.ObterAdocaoPorId(id);

        if (adocao is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adoção de id = {id} não encontrada." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adocao));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarAdocao(int id, [FromBody] AtualizarAdocaoDto adocaoDto)
    {
        var erros = await _adocaoService.ValidarAtualizacaoAdocao(adocaoDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar adoção!"));
        }

        var adocao = await _adocaoService.AtualizarAdocao(id, adocaoDto);

        if (adocao is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adoção de id = {id} não encontrada." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Adoção de id = {id} atualizada com sucesso!"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirAdocao(int id)
    {
        bool adocao = await _adocaoService.ExcluirAdocao(id);

        if (!adocao)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adoção de id = {id} não encontrada." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Adoção de id = {id} foi excluída com sucesso!"));
    }
}