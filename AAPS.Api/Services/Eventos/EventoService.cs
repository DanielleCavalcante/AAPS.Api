using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Evento;
using AAPS.Api.Models;
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
                Descricao = eventoDto.Descricao
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return new EventoDto
            {
                Id = evento.Id,
                Descricao = evento.Descricao
            };
        }

        public async Task<IEnumerable<EventoDto>> ObterEventos(FiltroEventoDto filtro)
        {
            var query = _context.Eventos.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Descricao))
            {
                query = query.Where(x => x.Descricao.Contains(filtro.Descricao));
            }

            var eventosDto = await query
                .Select(e => new EventoDto
                {
                    Id = e.Id,
                    Descricao = e.Descricao
                })
                .ToListAsync();

            return eventosDto;
        }

        public async Task<EventoDto?> AtualizarEvento(int id, AtualizarEventoDto eventoDto)
        {
            var evento = await BuscarEventoPorId(id);

            if (evento == null)
            {
                return null;
            }
            evento.Descricao = string.IsNullOrEmpty(eventoDto.Descricao) ? evento.Descricao : eventoDto.Descricao;

            await _context.SaveChangesAsync();

            return new EventoDto
            {
                Id = evento.Id,
                Descricao = evento.Descricao
            };
        }

        public async Task<bool> ExcluirEvento(int id)
        {
            var evento = await BuscarEventoPorId(id);

            if (evento == null)
            {
                return false;
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return true;
        }

        #region MÉTODOS PRIVADOS

        private async Task<Evento?> BuscarEventoPorId(int id)
        {
            return await _context.Eventos.FindAsync(id);
        }

        #endregion
    }
}