using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : GeralPersistence, IPalestrantePersistence
    {
        private readonly EntityContext _context;
        public PalestrantePersistence(EntityContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                                    .Include(p => p.RedesSociais)
                                                    .Include(p => p.User);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                                .ThenInclude(PE => PE.Evento);
            }

            query = query.Where(p => (p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                        p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                        p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                        p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                          .OrderBy(p => p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                                    .Include(p => p.RedesSociais)
                                                    .Include(p => p.User);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                                .ThenInclude(PE => PE.Evento);
            }

            query = query.OrderBy(p => p.Id)
                            .Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}