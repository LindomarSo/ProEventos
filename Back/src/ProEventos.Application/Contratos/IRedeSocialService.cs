using ProEventos.Application.Dtos;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto[]> SaveByEventoAsync(int eventoId, RedeSocialDto[] dto);
        Task<bool> DeleteByEventoAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDto[]> SaveByPalestranteAsync(int palestranteId, RedeSocialDto[] dto);
        Task<bool> DeleteByPalestranteAsync(int palestranteId, int redeSocialId);
        Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);
        Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);
    }
}
