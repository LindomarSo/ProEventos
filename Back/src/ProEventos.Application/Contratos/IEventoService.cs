using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> Update(int userId, int id, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);
         Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes);
         Task<EventoDto> GetAllEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}