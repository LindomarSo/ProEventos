using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model);
        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);
        Task<PageList<PalestranteDto>> GetAllPalestranteAsync(PageParams pageParams, bool includeEventos);
        Task<PalestranteDto> GetAllPalestranteByUserIdAsync(int userId, bool includeEventos);
    }
}