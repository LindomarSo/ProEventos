using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersistence : IGeralPersistence
    {
         Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos =  false);
         Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}