using AAPS.Api.Context;
using AAPS.Api.DTO;
using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AnimalService : IAnimalService
{
    private readonly AppDbContext _context;

    public AnimalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Animal>> ObterAnimais()
    {
        return await _context.Animais.ToListAsync();
    }

    public async Task<Animal> ObterAnimalPorId(int id)
    {
        var animal = await _context.Animais.FindAsync(id);
        return animal;
    }

    public async Task<IEnumerable<Animal>> ObterAnimalPorNome(string nome)
    {
        IEnumerable<Animal> animais;

        if (!string.IsNullOrEmpty(nome))
        {
            animais = await _context.Animais.Where(n => n.Nome.Contains(nome)).ToListAsync();
        }
        else
        {
            animais = await ObterAnimais();
        }
        return animais;
    }

    public async Task CriarAnimal(AnimalDto animalDto)
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
    }

    public async Task AtualizarAnimal([FromRoute] int id, AnimalDto animalDto)
    {
        var buscaRegistro = await _context.Animais.FindAsync(id);

        if (buscaRegistro == null)
        {
            throw new KeyNotFoundException($"Animal com Id {id} não foi encontrado.");
        }

        buscaRegistro.Nome = animalDto.Nome;
        buscaRegistro.Especie = animalDto.Especie;
        buscaRegistro.Raca = animalDto.Raca;
        buscaRegistro.Pelagem = animalDto.Pelagem;
        buscaRegistro.Sexo = animalDto.Sexo;
        buscaRegistro.DataNascimento = animalDto.DataNascimento;
        buscaRegistro.Status = animalDto.Status;
        buscaRegistro.DoadorId = animalDto.DoadorId;

        _context.Entry(buscaRegistro).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAnimal(int id)
    {
        var excluirRegistro = await _context.Animais.FindAsync(id); //verificar se id existe

        if (excluirRegistro != null) 
        {
            _context.Animais.Remove(excluirRegistro); //fazer validação de id antes de excluir
            _context.SaveChanges();
        }
        // colocar algum erro se for nulo
    }
}
