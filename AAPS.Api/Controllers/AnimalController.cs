using AAPS.Api.Dtos.Animais;
using AAPS.Api.Models;
using AAPS.Api.Services.Animais;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Padrao")]
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(animalDto.Nome)) // TODO: ver campos required
        {
            return BadRequest("O campo nome é obrigatório!");
        }

        var animal = await _animalService.CriarAnimal(animalDto);

        if (animal is null)
        {
            return StatusCode(500, "Erro ao criar o animal.");
        }

        return Ok($"Animal criado com sucesso!");
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimais()
    {
        var animais = await _animalService.ObterAnimais();

        if (animais is null)
        {
            return StatusCode(500, "Erro ao obter os animais.");
        }

        return Ok(animais);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterAnimalPorId(int id)
    {
        var animal = await _animalService.ObterAnimalPorId(id);

        if (animal is null)
        {
            return NotFound($"Animal de id = {id} não encontrado.");
        }

        return Ok(animal);
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimaisPorNome([FromQuery] string nome)
    {
        var animais = await _animalService.ObterAnimaisPorNome(nome);

        if (animais is null)
        {
            return NotFound($"Animal de nome = {nome} não encontrado.");
        }

        return Ok(animais);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarAnimal(int id, [FromBody] AtualizarAnimalDto animalDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var animal = await _animalService.AtualizarAnimal(id, animalDto);

        if (animal is null)
        {
            return NotFound($"Animal de id = {id} não encontrado.");
        }

        return Ok($"Animal atualizado com sucesso!");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> ExcluirAnimal(int id)
    {
        bool deletado = await _animalService.ExcluirAnimal(id);

        if (!deletado)
        {
            return NotFound($"Animal de id = {id} não encontrado.");

        }

        return Ok($"Animal de id = {id} foi excluído com sucesso!");
    }
}
