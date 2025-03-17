using AAPS.Api.Dtos.Adotantes;
using AAPS.Api.Models;
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
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(adotanteDto.Nome))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Rg))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Cpf))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.LocalTrabalho))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Status.ToString()))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Facebook))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Instagram))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Logradouro))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Numero.ToString()))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Complemento))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Bairro))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Uf))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Cidade))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.Cep.ToString()))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrWhiteSpace(adotanteDto.SituacaoEndereco))
            erros.Add("O campo 'Nome' é obrigatório!");

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar adotante!"));
        }

        var adotante = await _adotanteService.CriarAdotante(adotanteDto);

        if (adotante is null)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar o adotante." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotante, "Adotante criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Adotante>>> ObterAdotantes()
    {
        var adotantesDto = await _adotanteService.ObterAdotantes();

        if (adotantesDto is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum adotante foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotantesDto));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Adotante>>> ObterAdotantePorNome([FromQuery] string nome)
    {
        var adotantes = await _adotanteService.ObterAdotantePorNome(nome);

        if (adotantes is null)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao obter adotantes." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(adotantes));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarAdotante(int id, [FromBody] AtualizarAdotanteDto adotanteDto)
    {
        // TODO: validar campos required

        await _adotanteService.AtualizarAdotante(id, adotanteDto);

        if (adotanteDto is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adotante de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Adotante de id = {id} atualizado com sucesso!"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirAdotante(int id)
    {
        bool adotante = await _adotanteService.ExcluirAdotante(id);

        if (!adotante)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Adotante de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Adotante de id = {id} foi excluído com sucesso!"));
    }
}
