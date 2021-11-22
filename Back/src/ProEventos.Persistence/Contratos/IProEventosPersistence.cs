using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IProEventosPersistence
    {
         Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes);
         Task<Evento> GetAllEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}