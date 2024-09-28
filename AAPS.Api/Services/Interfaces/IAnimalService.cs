using AAPS.Api.DTO;
using AAPS.Api.Models;
using Microsoft.AspNetCore.Mvc;

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