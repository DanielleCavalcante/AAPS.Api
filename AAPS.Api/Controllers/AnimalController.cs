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
        var erros = new List<string>();

        if (string.IsNullOrEmpty(animalDto.Nome))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.Especie))
            erros.Add("O campo 'Especie' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.Raca))
            erros.Add("O campo 'Raca' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.Pelagem))
            erros.Add("O campo 'Pelagem' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.Sexo))
            erros.Add("O campo 'Sexo' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.DataNascimento.ToString()))
            erros.Add("O campo 'DataNascimento' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.Status.ToString()))
            erros.Add("O campo 'Status' é obrigatório!");
        if (string.IsNullOrEmpty(animalDto.DoadorId.ToString()) || animalDto.DoadorId <= 0)
            erros.Add("O campo 'Doador' é obrigatório!");

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar animal!"));
        }

        var animal = await _animalService.CriarAnimal(animalDto);

        if (animal is null)
        {
            return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { "Erro criar o animal." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(animal, "Animal criado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimais()
    {
        var animais = await _animalService.ObterAnimais();

        if (animais is null)
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
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao obter adotantes." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(animais));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarAnimal(int id, [FromBody] AtualizarAnimalDto animalDto)
    {
        // TODO: validar campos required

        var animal = await _animalService.AtualizarAnimal(id, animalDto);

        if (animal is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Animal de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Animal atualizado com sucesso!"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirAnimal(int id)
    {
        bool animal = await _animalService.ExcluirAnimal(id);

        if (!animal)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Animal de id = {id} não encontrado." }));

        }

        return Ok(ApiResponse<object>.SucessoResponse($"Animal de id = {id} foi excluído com sucesso!"));
    }
}