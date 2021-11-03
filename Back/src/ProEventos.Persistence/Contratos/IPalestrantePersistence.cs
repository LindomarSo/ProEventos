using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersistence
    {
          Task<Palestrante[]> GetAllPatestrantesByNomeAsync(string nome, bool includeEventos);
         Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
         Task<Palestrante> GetAllPatestranteByIdAsync(int palestranteId, bool includeEventos);
    }
}