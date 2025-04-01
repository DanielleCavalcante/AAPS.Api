using AAPS.Api.Dtos.Evento;

namespace AAPS.Api.Services.Eventos
{
    public interface IEventoService
    {
        Task<EventoDto> CriarEvento(CriarEventoDto eventoDto);
        Task<IEnumerable<EventoDto>> ObterEventos(FiltroEventoDto filtro);
        Task<EventoDto?> AtualizarEvento(int id, AtualizarEventoDto eventoDto);
        Task<bool> ExcluirEvento(int id);
    }
}