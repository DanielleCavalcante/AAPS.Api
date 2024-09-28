using AAPS.Api.DTO;
using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    [Route("ObterTodos")]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimais()
    {
        var animaisDto = await _animalService.ObterAnimais();
        return Ok(animaisDto);
    }

    [HttpGet]
    [Route("ObterUmAnimal/{id:int}", Name = "ObterAnimaisPorId")]
    public async Task<ActionResult<Animal>> ObterAnimaisPorId(int id)
    {
        var animalDto = await _animalService.ObterAnimalPorId(id);
        return Ok(animalDto);
    }

    [HttpGet]
    [Route("ObterAnimaisPorNome")]
    public async Task<ActionResult<IAsyncEnumerable<Animal>>> ObterAnimaisPorNome([FromQuery] string nome)
    {
        var animais = await _animalService.ObterAnimalPorNome(nome);
        return Ok(animais);
    }

    [HttpPost]
    [Route("Criar")]
    public async Task CriarAnimal(AnimalDto animalDto)
    {
        await _animalService.CriarAnimal(animalDto);
    }

    [HttpPut]
    [Route("Atualizar/{id:int}")]
    public async Task AtualizarAnimal(int id, AnimalDto animalDto)
    {
        await _animalService.AtualizarAnimal(id, animalDto);
    }

    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<ActionResult> ExcluirAnimal(int id)
    {
        await _animalService.ExcluirAnimal(id);
        return Ok($"Animal de id = {id} foi excluído com sucesso!");
    }
}
