using AAPS.Api.Context;
using AAPS.Api.Dtos.Animais;
using AAPS.Api.Models;
using AAPS.Api.Services.Animais;
using Microsoft.EntityFrameworkCore;

public class AnimalService : IAnimalService
{

    #region ATRIBUTOS E CONSTRUTOR
    private readonly AppDbContext _context;

    public AnimalService(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    public async Task<AnimalDto> CriarAnimal(CriarAnimalDto animalDto)
    {
        var animal = new Animal
        {
            Nome = animalDto.Nome,
            Especie = animalDto.Especie,
            Raca = animalDto.Raca,
            Pelagem = animalDto.Pelagem,
            Sexo = animalDto.Sexo,
            DataNascimento = animalDto.DataNascimento,
            Status = animalDto.Status,
            DoadorId = animalDto.DoadorId
        };

        _context.Animais.Add(animal);
        await _context.SaveChangesAsync();

        return new AnimalDto
        {
            Id = animal.Id,
            Nome = animal.Nome,
            Especie = animal.Especie,
            Raca = animal.Raca,
            Pelagem = animal.Pelagem,
            Sexo = animal.Sexo,
            DataNascimento = animal.DataNascimento,
            Status = animal.Status,
            DoadorId = animal.DoadorId
        };
    }

    public async Task<IEnumerable<Animal>> ObterAnimais()
    {
        return await _context.Animais.ToListAsync();
    }

    public async Task<AnimalDto?> ObterAnimalPorId(int id)
    {
        var animal = await BuscarAnimalPorId(id);

        if (animal == null)
        {
            return null;
        }

        return new AnimalDto
        {
            Id = animal.Id,
            Nome = animal.Nome,
            Especie = animal.Especie,
            Raca = animal.Raca,
            Pelagem = animal.Pelagem,
            Sexo = animal.Sexo,
            DataNascimento = animal.DataNascimento,
            Status = animal.Status,
            DoadorId = animal.DoadorId
        };
    }

    public async Task<IEnumerable<Animal>> ObterAnimaisPorNome(string nome)
    {
        return await BuscarAnimaisPorNome(nome).ToListAsync();
    }

    public async Task<AnimalDto?> AtualizarAnimal(int id, AtualizarAnimalDto animalDto)
    {
        var animal = await BuscarAnimalPorId(id);

        if (animal is null)
        {
            return null;
        }

        animal.Nome = string.IsNullOrEmpty(animalDto.Nome) ? animal.Nome : animalDto.Nome;
        animal.Especie = string.IsNullOrEmpty(animalDto.Especie) ? animal.Especie : animalDto.Especie;
        animal.Raca = string.IsNullOrEmpty(animalDto.Raca) ? animal.Raca : animalDto.Raca;
        animal.Pelagem = string.IsNullOrEmpty(animalDto.Pelagem) ? animal.Pelagem : animalDto.Pelagem;
        animal.Sexo = string.IsNullOrEmpty(animalDto.Sexo) ? animal.Sexo : animalDto.Sexo;
        animal.DataNascimento = animalDto.DataNascimento.HasValue ? animalDto.DataNascimento.Value : animal.DataNascimento;
        animal.Status = animalDto.Status.HasValue ? animalDto.Status.Value : animal.Status;
        animal.DoadorId = animalDto.DoadorId.HasValue ? animalDto.DoadorId.Value : animal.DoadorId;

        await _context.SaveChangesAsync();

        var animalAtualizado = new AnimalDto
        {
            Id = animal.Id,
            Nome = animal.Nome,
            Especie = animal.Especie,
            Raca = animal.Raca,
            Pelagem = animal.Pelagem,
            Sexo = animal.Sexo,
            DataNascimento = animal.DataNascimento,
            Status = animal.Status,
            DoadorId = animal.DoadorId
        };

        return animalAtualizado;
    }
    public async Task<bool> ExcluirAnimal(int id)
    {
        var animal = await BuscarAnimalPorId(id);

        if (animal is null)
        {
            return false;
        }

        _context.Animais.Remove(animal);
        await _context.SaveChangesAsync();

        return true;
    }

    #region MÉTODOS PRIVADOS

    private async Task<Animal?> BuscarAnimalPorId(int id)
    {
        var animal = await _context.Animais.FindAsync(id);
        return animal;
    }

    private IQueryable<Animal> BuscarAnimaisPorNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return _context.Animais;
        }

        return _context.Animais.Where(a => a.Nome.ToLower().Contains(nome.ToLower()));
    }

    #endregion
}
