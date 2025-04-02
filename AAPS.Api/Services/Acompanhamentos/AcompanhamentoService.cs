using AAPS.Api.Context;
using AAPS.Api.Dtos.Acompanhamento;
using AAPS.Api.Dtos.Animais;
using AAPS.Api.Models;
using AAPS.Api.Services.Animais;
using AAPS.Api.Services.Eventos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AAPS.Api.Services.Acompanhamentos
{
    public class AcompanhamentoService : IAcompanhamentoService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;
        private readonly IAnimalService _animalService;
        private readonly IEventoService _eventoService;

        public AcompanhamentoService(AppDbContext contexxt, IAnimalService animalService, IEventoService eventoService)
        {
            _context = contexxt;
            _animalService = animalService;
            _eventoService = eventoService;
        }

        #endregion

        public async Task<AcompanhamentoDto> CriarAcompanhamento(CriarAcompanhamentoDto acompanhamentoDto)
        {
            var acompanhamento = new AnimalEvento
            {
                Data = acompanhamentoDto.Data,
                Observacao = acompanhamentoDto.Observacao,
                AnimalId = acompanhamentoDto.AnimalId,
                EventoId = acompanhamentoDto.EventoId
            };

            _context.AnimalEvento.Add(acompanhamento);
            await _context.SaveChangesAsync();

            return new AcompanhamentoDto
            {
                Id = acompanhamento.Id,
                Data = acompanhamento.Data,
                Observacao = acompanhamento.Observacao,
                AnimalId = acompanhamento.AnimalId,
                EventoId = acompanhamento.EventoId
            };
        }

        public async Task<IEnumerable<AcompanhamentoDto>> ObterAcompanhamentos()
        {
            var acompanhamentos = await _context.AnimalEvento
                .Select(a => new AcompanhamentoDto
                {
                    Id = a.Id,
                    Data = a.Data,
                    Observacao = a.Observacao,
                    AnimalId = a.AnimalId,
                    EventoId = a.EventoId
                })
                .ToListAsync();

            return acompanhamentos;
        }

        public async Task<AcompanhamentoDto> ObterAcompanhamentoPorId(int id)
        {
            var acompanhamento = await BuscarAcompanhamentoPorId(id);

            if (acompanhamento == null)
            {
                return null;
            }

            return new AcompanhamentoDto
            {
                Id = acompanhamento.Id,
                Data = acompanhamento.Data,
                Observacao = acompanhamento.Observacao,
                AnimalId = acompanhamento.AnimalId,
                EventoId = acompanhamento.EventoId
            };
        }

        public async Task<bool> ExcluirAcompanhamento(int id)
        {
            var acompanhamento = await BuscarAcompanhamentoPorId(id);

            if(acompanhamento == null)
            {
                return false;
            }

            _context.AnimalEvento.Remove(acompanhamento);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoAcompanhamento(CriarAcompanhamentoDto acompanhamentoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(acompanhamentoDto.Data.ToString()))
                erros.Add("O campo 'Data' é obrigatório!");

            if (string.IsNullOrEmpty(acompanhamentoDto.AnimalId.ToString()) || acompanhamentoDto.AnimalId <= 0)
                erros.Add("O campo 'Animal' é obrigatório!");

            if (string.IsNullOrEmpty(acompanhamentoDto.EventoId.ToString()) || acompanhamentoDto.EventoId <= 0)
                erros.Add("O campo 'Evento' é obrigatório!");

            var acompanhamentoExistente = await _context.AnimalEvento
            .Where(a =>
                a.Data == acompanhamentoDto.Data &&
                a.AnimalId == acompanhamentoDto.AnimalId &&
                a.EventoId == acompanhamentoDto.EventoId 
            )
            .FirstOrDefaultAsync();

            if (acompanhamentoExistente != null)
            {
                erros.Add($"Acompanhamento já cadastrado. Código {acompanhamentoExistente.Id}");
            }

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<AnimalEvento?> BuscarAcompanhamentoPorId(int id)
        {
            var acompanhamento = await _context.AnimalEvento.FindAsync(id);
            return acompanhamento;
        }

        #endregion
    }
}
