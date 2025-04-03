using AAPS.Api.Context;
using AAPS.Api.Dtos.Acompanhamento;
using AAPS.Api.Models;
using AAPS.Api.Services.Animais;
using AAPS.Api.Services.Eventos;
using Microsoft.EntityFrameworkCore;

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
            var acompanhamento = new Acompanhamento
            {
                Data = acompanhamentoDto.Data,
                Observacao = acompanhamentoDto.Observacao,
                AnimalId = acompanhamentoDto.AnimalId,
                EventoId = acompanhamentoDto.EventoId
            };

            _context.Acompanhamentos.Add(acompanhamento);
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
            var acompanhamentos = await _context.Acompanhamentos
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

            if (acompanhamento == null)
            {
                return false;
            }

            _context.Acompanhamentos.Remove(acompanhamento);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoAcompanhamento(CriarAcompanhamentoDto acompanhamentoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(acompanhamentoDto.Data.ToString()) || acompanhamentoDto.Data == DateTime.MinValue)
                erros.Add("O campo 'Data' é obrigatório!");

            if (string.IsNullOrEmpty(acompanhamentoDto.AnimalId.ToString()) || acompanhamentoDto.AnimalId <= 0)
                erros.Add("O campo 'Animal' é obrigatório!");

            if (string.IsNullOrEmpty(acompanhamentoDto.EventoId.ToString()) || acompanhamentoDto.EventoId <= 0)
                erros.Add("O campo 'Evento' é obrigatório!");

            var acompanhamentoExistente = await _context.Acompanhamentos
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

        private async Task<Acompanhamento?> BuscarAcompanhamentoPorId(int id)
        {
            var acompanhamento = await _context.Acompanhamentos.FindAsync(id);
            return acompanhamento;
        }

        #endregion
    }
}
