using AAPS.Api.Dtos.Doadores;
using AAPS.Api.Models;
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(doadorDto.Nome)) // || TODO: ver campos required
        {
            return BadRequest("O campo nome é obrigatório!");
        }

        var doador = await _doadorService.CriarDoador(doadorDto);

        if (doador is null)
        {
            return StatusCode(500, "Erro ao criar o doador.");
        }

        return Ok($"Doador criado com sucesso!");
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Doador>>> ObterDoadores()
    {
        var doadores = await _doadorService.ObterDoadores();

        if (doadores is null)
        {
            return StatusCode(500, "Erro ao obter os doadores.");
        }

        return Ok(doadores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterDoadorPorId(int id)
    {
        var doador = await _doadorService.ObterDoadorPorId(id);

        if (doador is null)
        {
            return NotFound($"Doador de id = {id} não encontrado.");
        }

        return Ok(doador);
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Doador>>> ObterDoadorPorNome([FromQuery] string nome)
    {
        var doadores = await _doadorService.ObterDoadorPorNome(nome);

        if (doadores is null)
        {
            return NotFound($"Doador de nome = {nome} não encontrado.");
        }

        return Ok(doadores);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarDoador(int id, [FromBody] AtualizarDoadorDto doadorDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var doador = await _doadorService.AtualizarDoador(id, doadorDto);

        if (doador is null)
        {
            return NotFound($"Doador de id = {id} não encontrado.");
        }

        return Ok($"Doador atualizado com sucesso!");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirDoador(int id)
    {
        bool deletado = await _doadorService.ExcluirDoador(id);

        if (!deletado)
        {
            return NotFound($"Doador de id = {id} não encontrado.");
        }

        return Ok($"Doador de id = {id} foi excluído com sucesso!");
        // return NoContent();
    }
}
