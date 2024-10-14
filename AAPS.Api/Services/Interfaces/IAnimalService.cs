using AAPS.Api.Dtos;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Interfaces;

public interface IAnimalService
{
    Task<IEnumerable<Animal>> ObterAnimais();
    Task<Animal> ObterAnimalPorId(int id);
    Task<IEnumerable<Animal>> ObterAnimalPorNome(string nome);
    Task CriarAnimal(AnimalDto animalDto);
    Task AtualizarAnimal(int id, AnimalDto animalDto);
    Task ExcluirAnimal(int id);
}