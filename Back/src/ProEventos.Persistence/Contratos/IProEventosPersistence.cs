using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IProEventosPersistence
    {
         Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
         Task<Evento> GetAllEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}