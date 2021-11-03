using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(EventoDto model);
        Task<EventoDto> Update(int id, EventoDto model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
         Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes);
         Task<EventoDto> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes);
    }
}