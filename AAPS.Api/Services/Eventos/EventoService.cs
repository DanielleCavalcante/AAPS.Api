using AAPS.Api.Context;
using AAPS.Api.Dtos.Evento;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Eventos
{
    public class EventoService : IEventoService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;

        public EventoService(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<EventoDto> CriarEvento(CriarEventoDto eventoDto)
        {
            var evento = new Evento
            {
                Descricao = eventoDto.Descricao,
                Status = eventoDto.Status
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return new EventoDto
            {
                Id = evento.Id,
                Descricao = evento.Descricao,
                Status = evento.Status
            };
        }

        public async Task<IEnumerable<EventoDto>> ObterEventos(FiltroEventoDto filtro)
        {
            var query = _context.Eventos.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                string buscaLower = filtro.Busca.ToLower();

                query = query.Where(e => 
                    e.Descricao.ToLower().Contains(filtro.Busca.ToLower()) ||
                    e.Id.ToString() == buscaLower
                );
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(a => a.Status == filtro.Status.Value);
            }

            var eventosDto = await query
                .Select(e => new EventoDto
                {
                    Id = e.Id,
                    Descricao = e.Descricao,
                    Status = e.Status
                })
                .ToListAsync();

            return eventosDto;
        }

        public async Task<EventoDto?> ObterEventoPorId(int id)
        {
            var evento = await BuscarEventoPorId(id);

            if (evento == null)
            {
                return null;
            }

            return new EventoDto
            {
                Id = evento.Id,
                Descricao = evento.Descricao,
                Status = evento.Status,
            };
        }

        public async Task<IEnumerable<EventoDto>> ObterEventosAtivos()
        {
            var eventos = _context.Eventos
                .Where(e => e.Status == StatusEnum.Ativo)
                .Select(e => new EventoDto
                {
                    Id = e.Id,
                    Descricao = e.Descricao,
                    Status = e.Status,
                });

            return await eventos.ToListAsync();
        }

        public async Task<EventoDto?> AtualizarEvento(int id, AtualizarEventoDto eventoDto)
        {
            var evento = await BuscarEventoPorId(id);

            if (evento == null)
            {
                return null;
            }

            evento.Descricao = string.IsNullOrEmpty(eventoDto.Descricao) ? evento.Descricao : eventoDto.Descricao;
            evento.Status = eventoDto.Status.HasValue ? eventoDto.Status.Value : evento.Status;

            await _context.SaveChangesAsync();

            return new EventoDto
            {
                Id = evento.Id,
                Descricao = evento.Descricao,
                Status = evento.Status
            };
        }

        public async Task<bool> ExcluirEvento(int id)
        {
            var evento = await BuscarEventoPorId(id);

            if (evento == null)
            {
                return false;
            }

            evento.Status = StatusEnum.Inativo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoEvento(CriarEventoDto eventoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(eventoDto.Descricao.ToString()))
                erros.Add("O campo 'Descrição' é obrigatório!");

            if (string.IsNullOrEmpty(eventoDto.Status.ToString()) || !Enum.IsDefined(typeof(StatusEnum), eventoDto.Status))
                erros.Add("O campo 'Status' é obrigatório!");

            var eventoExistente = await _context.Eventos
                .Where(a => a.Descricao == eventoDto.Descricao)
                .FirstOrDefaultAsync();

            if (eventoExistente != null)
            {
                erros.Add($"Evento já cadastrado. Código {eventoExistente.Id}");
            }

            return erros;
        }

        public async Task<List<string>> ValidarAtualizacaoEvento(int id, AtualizarEventoDto eventoDto)
        {
            var erros = new List<string>();

            if (eventoDto.Descricao != null && string.IsNullOrWhiteSpace(eventoDto.Descricao))
                erros.Add("O campo 'Descrição' não pode ser vazio!");

            if (eventoDto.Status != null && string.IsNullOrWhiteSpace(eventoDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ter ser vazio!");

            var eventoExistente = await _context.Eventos
                .Where(a => a.Descricao == eventoDto.Descricao && a.Id != id)
                .FirstOrDefaultAsync();

            if (eventoExistente != null)
            {
                erros.Add($"Evento já cadastrado. Código {eventoExistente.Id}");
            }

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<Evento?> BuscarEventoPorId(int id)
        {
            return await _context.Eventos.FindAsync(id);
        }

        #endregion
    }
}