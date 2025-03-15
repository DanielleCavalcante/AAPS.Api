using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Models;
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
    private readonly IAdotanteService _adotanteService;

    public AdotanteController(IAdotanteService adotanteService)
    {
        _adotanteService = adotanteService;
    }

    [HttpGet]
    [Route("ObterTodosAdotantes")]
    public async Task<ActionResult<IAsyncEnumerable<Adotante>>> ObterAdotantes()
    {
        var adotantesDto = await _adotanteService.ObterAdotantes();
        return Ok(adotantesDto);
    }

    [HttpGet]
    [Route("ObterAdotantesPorNome")]
    public async Task<ActionResult<IAsyncEnumerable<Adotante>>> ObterAdotantePorNome(string nome)
    {
        var adotantes = await _adotanteService.ObterAdotantePorNome(nome);
        return Ok(adotantes);
    }

    [HttpPost]
    [Route("Criar")]
    public async Task CriarAdotante(AdotanteDto adotanteDto)
    {
        await _adotanteService.CriarAdotante(adotanteDto);
    }

    [HttpPut]
    [Route("Atualizar/{id:int}")]
    public async Task AtualizarAdotante(int id, AdotanteDto adotanteDto)
    {
        await _adotanteService.AtualizarAdotante(id, adotanteDto);
    }

    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<ActionResult> ExcluirAdotante(int id)
    {
        await _adotanteService.ExcluirAdotante(id);
        return Ok($"Adotante de id = {id} foi excluído com sucesso!");
    }
}
