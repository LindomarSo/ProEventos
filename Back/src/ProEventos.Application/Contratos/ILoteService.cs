using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface ILoteService
    {
        Task<LoteDto[]> SaveLote(int id, LoteDto[] model);
        Task<bool> DeleteLote(int LoteId, int eventoId);
        Task<LoteDto[]> GetAllLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteByIdsAsync(int LoteId, int eventoId);
    }
}