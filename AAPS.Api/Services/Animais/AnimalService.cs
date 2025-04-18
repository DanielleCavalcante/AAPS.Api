using AAPS.Api.Context;
using AAPS.Api.Dtos.Animal;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Doadores;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Animais
{
    public class AnimalService : IAnimalService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;
        private readonly IDoadorService _doadorService;

        public AnimalService(AppDbContext context, IDoadorService doadorService)
        {
            _context = context;
            _doadorService = doadorService;
        }

        #endregion

        public async Task<AnimalDto> CriarAnimal(CriarAnimalDto animalDto)
        {
            var doador = await _doadorService.ObterDoadorPorId(animalDto.DoadorId);

            if (doador == null || doador.Status == StatusEnum.Inativo)
            {
                return null;
            }

            var animal = new Animal
            {
                Nome = animalDto.Nome,
                Especie = animalDto.Especie,
                Raca = animalDto.Raca,
                Pelagem = animalDto.Pelagem,
                Sexo = animalDto.Sexo,
                DataNascimento = animalDto.DataNascimento,
                Status = animalDto.Status,
                Disponibilidade = animalDto.Disponibilidade,
                PessoaId = doador.Id,
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
                Disponibilidade = animal.Disponibilidade,
                DoadorId = animal.PessoaId,
            };
        }

        public async Task<IEnumerable<AnimalDto>> ObterAnimais(FiltroAnimalDto filtro)
        {
            var query = _context.Animais.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                query = query.Where(a => a.Nome.Contains(filtro.Busca.ToLower()));
            }

            if (!string.IsNullOrEmpty(filtro.Especie))
            {
                query = query.Where(a => a.Especie.ToLower() == filtro.Especie.ToLower());
            }

            if (!string.IsNullOrEmpty(filtro.Sexo))
            {
                query = query.Where(a => a.Sexo.ToLower() == filtro.Sexo.ToLower());
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(a => a.Status == filtro.Status.Value);
            }

            if (filtro.Disponibilidade.HasValue)
            {
                query = query.Where(a => a.Disponibilidade == filtro.Disponibilidade.Value);
            }

            var animaisDto = await query
                .Select(a => new AnimalDto
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Especie = a.Especie,
                    Raca = a.Raca,
                    Pelagem = a.Pelagem,
                    Sexo = a.Sexo,
                    DataNascimento = a.DataNascimento,
                    Status = a.Status,
                    Disponibilidade = a.Disponibilidade,
                    DoadorId = a.PessoaId,
                })
                .ToListAsync();

            return animaisDto;
        }

        public async Task<AnimalDto?> ObterAnimalPorId(int id)
        {
            var animal = await BuscarAnimalPorId(id);

            var doador = await _doadorService.ObterDoadorPorId(animal.PessoaId);

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
                Disponibilidade = animal.Disponibilidade,
                DoadorId = animal.PessoaId,
            };
        }

        public async Task<IEnumerable<AnimalDto>> ObterAnimaisAtivos()
        {
            var animais = _context.Animais
                .Where(a => 
                    a.Status == StatusEnum.Ativo && 
                    a.Disponibilidade == DisponibilidadeEnum.Disponivel)
                .Select(a => new AnimalDto
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Especie = a.Especie,
                    Raca = a.Raca,
                    Pelagem = a.Pelagem,
                    Sexo = a.Sexo,
                    DataNascimento = a.DataNascimento,
                    Status = a.Status,
                    DoadorId = a.PessoaId,
                    Disponibilidade = a.Disponibilidade,
                });

            return await animais.ToListAsync();
        }

        public async Task<AnimalDto> AtualizarAnimal(int id, AtualizarAnimalDto animalDto)
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
            animal.PessoaId = animalDto.DoadorId.HasValue ? animalDto.DoadorId.Value : animal.PessoaId;
            animal.Disponibilidade = animalDto.Disponibilidade.HasValue ? animalDto.Disponibilidade.Value : animal.Disponibilidade;

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
                Disponibilidade = animal.Disponibilidade,
                DoadorId = animal.PessoaId,
            };
        }

        public async Task<bool> ExcluirAnimal(int id)
        {
            var animal = await BuscarAnimalPorId(id);

            if (animal is null)
            {
                return false;
            }

            animal.Status = StatusEnum.Inativo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoAnimal(CriarAnimalDto animalDto)
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
            if (string.IsNullOrEmpty(animalDto.Status.ToString()) || !Enum.IsDefined(typeof(StatusEnum), animalDto.Status))
                erros.Add("O campo 'Status' é obrigatório!");
            if (string.IsNullOrEmpty(animalDto.Disponibilidade.ToString()) || !Enum.IsDefined(typeof(DisponibilidadeEnum), animalDto.Disponibilidade))
                erros.Add("O campo 'Status' é obrigatório!");
            if (string.IsNullOrEmpty(animalDto.DoadorId.ToString()) || animalDto.DoadorId <= 0)
                erros.Add("O campo 'Doador' é obrigatório!");

            var animalExistente = await _context.Animais
                .Where(a =>
                    a.Nome == animalDto.Nome &&
                    a.Especie == animalDto.Especie &&
                    a.Raca == animalDto.Raca &&
                    a.Pelagem == animalDto.Pelagem &&
                    a.Sexo == animalDto.Sexo
                )
                .FirstOrDefaultAsync();

            if (animalExistente != null)
            {
                erros.Add($"Animal já cadastrado. Código {animalExistente.Id}");
            }

            return erros;
        }

        public List<string> ValidarAtualizacaoAnimal(AtualizarAnimalDto animalDto)
        {
            var erros = new List<string>();

            if (animalDto.Nome != null && string.IsNullOrWhiteSpace(animalDto.Nome))
                erros.Add("O campo 'Nome' não pode ter ser vazio!");
            if (animalDto.Especie != null && string.IsNullOrWhiteSpace(animalDto.Especie))
                erros.Add("O campo 'Especie' não pode ter ser vazio!");
            if (animalDto.Raca != null && string.IsNullOrWhiteSpace(animalDto.Raca))
                erros.Add("O campo 'Raca' não pode ter ser vazio!");
            if (animalDto.Pelagem != null && string.IsNullOrWhiteSpace(animalDto.Pelagem))
                erros.Add("O campo 'Pelagem' não pode ter ser vazio!");
            if (animalDto.Sexo != null && string.IsNullOrWhiteSpace(animalDto.Sexo))
                erros.Add("O campo 'Sexo' não pode ter ser vazio!");
            if (animalDto.Status != null && string.IsNullOrWhiteSpace(animalDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ter ser vazio!");
            if (animalDto.Disponibilidade != null && string.IsNullOrWhiteSpace(animalDto.Disponibilidade.ToString()))
                erros.Add("O campo 'Status' não pode ter ser vazio!");
            if (animalDto.DoadorId != null && string.IsNullOrWhiteSpace(animalDto.DoadorId.ToString()))
                erros.Add("O campo 'Doador' não pode ter ser vazio!");

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<Animal?> BuscarAnimalPorId(int id)
        {
            var animal = await _context.Animais.FindAsync(id);
            return animal;
        }

        #endregion
    }
}