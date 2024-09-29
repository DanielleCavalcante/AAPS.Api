using AAPS.Api.Dto;
using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DoadorController : Controller
{
    private readonly IDoadorService _doadorService;

    public DoadorController(IDoadorService doadorService)
    {
        _doadorService = doadorService;
    }

    [HttpGet]
    [Route("ObterTodosDoador")]
    public async Task<ActionResult<IAsyncEnumerable<Doador>>> ObterDoadores()
    {
        var doadoresDto = await _doadorService.ObterDoadores();
        return Ok(doadoresDto);
    }

    [HttpGet]
    [Route("ObterDoadoresPorNome")]
    public async Task<ActionResult<IAsyncEnumerable<Doador>>> ObterDoadorPorNome(string nome)
    {
        var doadores = await _doadorService.ObterDoadorPorNome(nome);
        return Ok(doadores);
    }

    [HttpPost]
    [Route("Criar")]
    public async Task CriarDoador(DoadorDto doadorDto)
    {
        await _doadorService.CriarDoador(doadorDto);
    }

    [HttpPut]
    [Route("Atualizar/{id:int}")]
    public async Task AtualizarDoador(int id, DoadorDto doadorDto)
    {
        await _doadorService.AtualizarDoador(id, doadorDto);
    }

    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<ActionResult> ExcluirDoador(int id)
    {
        await _doadorService.ExcluirDoador(id);
        return Ok($"Doador de id = {id} foi excluído com sucesso!");
    }
}
