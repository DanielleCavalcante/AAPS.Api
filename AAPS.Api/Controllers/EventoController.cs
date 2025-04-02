using AAPS.Api.Dtos.Acompanhamento;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Evento;
using AAPS.Api.Models;
using AAPS.Api.Responses;
using AAPS.Api.Services.Adotantes;
using AAPS.Api.Services.Eventos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EventoController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IEventoService _eventoService;

    public EventoController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarEvento([FromBody] CriarEventoDto eventoDto)
    {
        var erros = await _eventoService.ValidarCriacaoEvento(eventoDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar evento!"));
        }

        var evento = await _eventoService.CriarEvento(eventoDto);

        if (evento is null)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar evento." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(evento, "Evento criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Evento>>> ObterEventos([FromQuery] FiltroEventoDto filtro)
    {
        var eventos = await _eventoService.ObterEventos(filtro);

        if (eventos is null || !eventos.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum evento foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(eventos));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterEventoPorId(int id)
    {
        var evento = await _eventoService.ObterEventoPorId(id);

        if (evento is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Evento de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(evento));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> AtualizarEvento(int id, [FromBody] AtualizarEventoDto eventoDto)
    {
        var erros = await _eventoService.ValidarAtualizacaoEvento(eventoDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar evento!"));
        }

        var evento = await _eventoService.AtualizarEvento(id, eventoDto);

        if (evento is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Evento não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(evento, "Evento atualizado com sucesso!"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> ExcluirEvento(int id)
    {
        var evento = await _eventoService.ExcluirEvento(id);

        if (!evento)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Evento não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(null, "Evento excluído com sucesso!"));
    }
}
