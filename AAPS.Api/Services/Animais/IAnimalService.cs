using AAPS.Api.Dtos.Animal;

namespace AAPS.Api.Services.Animais;

public interface IAnimalService
{
    Task<AnimalDto> CriarAnimal(CriarAnimalDto animalDto);
    Task<IEnumerable<AnimalDto>> ObterAnimais(FiltroAnimalDto filtro);
    Task<AnimalDto?> ObterAnimalPorId(int id);
    Task<IEnumerable<AnimalDto>> ObterAnimaisAtivos();
    Task<AnimalDto?> AtualizarAnimal(int id, AtualizarAnimalDto animalDto);
    Task<bool> ExcluirAnimal(int id);
    Task<List<string>> ValidarCriacaoAnimal(CriarAnimalDto animalDto);
    List<string> ValidarAtualizacaoAnimal(AtualizarAnimalDto animalDto);
}