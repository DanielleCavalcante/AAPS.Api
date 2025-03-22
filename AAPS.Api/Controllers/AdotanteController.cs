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

        if (string.IsNullOrEmpty(adotanteDto.Nome))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Rg))
            erros.Add("O campo 'RG' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Cpf))
            erros.Add("O campo 'CPF' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.LocalTrabalho))
            erros.Add("O campo 'Local de Trabalho' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Status.ToString()))
            erros.Add("O campo 'Status' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Facebook))
            erros.Add("O campo 'Facebook' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Instagram))
            erros.Add("O campo 'Instagram' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Logradouro))
            erros.Add("O campo 'Logradouro' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Numero.ToString()) || adotanteDto.Numero <= 0)
            erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
        if (string.IsNullOrEmpty(adotanteDto.Bairro))
            erros.Add("O campo 'Bairro' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Uf) || adotanteDto.Uf.Length != 2)
            erros.Add("O campo 'UF' é obrigatório!");
        if (string.IsNullOrEmpty(adotanteDto.Cidade))
            erros.Add("O campo 'Cidade' é obrigatório!");
        if (adotanteDto.Cep <= 0 || adotanteDto.Cep.ToString().Length != 8)
            erros.Add("O campo 'CEP' é obrigatório e deve ter exatamente 8 dígitos!");
        if (string.IsNullOrEmpty(adotanteDto.SituacaoEndereco))
            erros.Add("O campo 'Situacao de Endereco' é obrigatório!");

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