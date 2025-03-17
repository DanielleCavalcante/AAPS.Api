using AAPS.Api.Dtos.Doadores;
using AAPS.Api.Models;
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
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(doadorDto.Nome))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Rg))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Cpf))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Logradouro))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Numero.ToString()))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Complemento))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Bairro))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Uf))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Cidade))
            return BadRequest("O campo nome é obrigatório!");
        if (string.IsNullOrWhiteSpace(doadorDto.Cep.ToString()))
            return BadRequest("O campo nome é obrigatório!");

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
    public async Task<ActionResult<IAsyncEnumerable<Doador>>> ObterDoadores()
    {
        var doadores = await _doadorService.ObterDoadores();

        if (doadores is null)
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
    public async Task<ActionResult<IAsyncEnumerable<Doador>>> ObterDoadorPorNome([FromQuery] string nome)
    {
        var doadores = await _doadorService.ObterDoadorPorNome(nome);

        if (doadores is null)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao obter doadores." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(doadores));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarDoador(int id, [FromBody] AtualizarDoadorDto doadorDto)
    {
        // TODO: validar campos required

        var doador = await _doadorService.AtualizarDoador(id, doadorDto);

        if (doador is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Doador de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Doador atualizado com sucesso!"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirDoador(int id)
    {
        bool doador = await _doadorService.ExcluirDoador(id);

        if (!doador)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Doador de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Doador de id = {id} foi excluído com sucesso!"));
    }
}