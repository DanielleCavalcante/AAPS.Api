using AAPS.Api.Dtos.Animais;
using AAPS.Api.Models;
using AAPS.Api.Responses;
using AAPS.Api.Services.Animais;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AnimalController : ControllerBase
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IAnimalService _animalService;

    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarAnimal([FromBody] CriarAnimalDto animalDto)
    {
        var erros = await _animalService.ValidarCriacaoAnimal(animalDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar animal!"));
        }

        var animal = await _animalService.CriarAnimal(animalDto);

        if (animal is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Animal com Id: {animalDto.DoadorId} não encontrado ou não está ativo" }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(animal, "Animal criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimais([FromQuery] FiltroAnimalDto filtro)
    {
        var animais = await _animalService.ObterAnimais(filtro);

        if (animais is null || !animais.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum animal foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(animais));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterAnimalPorId(int id)
    {
        var animal = await _animalService.ObterAnimalPorId(id);

        if (animal is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Animal de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(animal));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimaisPorNome([FromQuery] string nome)
    {
        var animais = await _animalService.ObterAnimaisPorNome(nome);

        if (animais is null)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao obter animais." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(animais));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarAnimal(int id, [FromBody] AtualizarAnimalDto animalDto)
    {
        var erros = _animalService.ValidarAtualizacaoAnimal(animalDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar animal!"));
        }

        var animal = await _animalService.AtualizarAnimal(id, animalDto);

        if (animal is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Animal de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Animal atualizado com sucesso!"));
    }

    //[HttpDelete("{id:int}")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> ExcluirAnimal(int id)
    {
        var animal = await _animalService.ExcluirAnimal(id);

        if (!animal)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Animal de id = {id} não encontrado." }));

        }

        return Ok(ApiResponse<object>.SucessoResponse(animal, $"Animal de id = {id} foi inativado com sucesso!"));
    }
}