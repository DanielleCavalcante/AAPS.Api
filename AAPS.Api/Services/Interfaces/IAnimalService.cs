using AAPS.Api.Dtos.Animais;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Interfaces;

public interface IAnimalService
{
    Task<AnimalDto> CriarAnimal(AnimalDto animalDto);
    Task<IEnumerable<Animal>> ObterAnimais();
    Task<AnimalDto?> ObterAnimalPorId(int id);
    Task<IEnumerable<Animal>> ObterAnimaisPorNome(string nome);
    Task<AnimalDto?> AtualizarAnimal(int id, AnimalDto animalDto);
    Task<bool> ExcluirAnimal(int id);
}