using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IRedeSocialPersistence : IGeralPersistence
    {
        Task<RedeSocial> GetRedeSocialByEventoIdsAsync(int eventoId, int id);
        Task<RedeSocial> GetRedeSocialByPalestranteIdsAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdsAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdsAsync(int palestranteId);
    }
}
