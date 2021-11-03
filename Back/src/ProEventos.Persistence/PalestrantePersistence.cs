using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : IPalestrantePersistence
    {
        private readonly EntityContext _context;
        public PalestrantePersistence(EntityContext context)
        {
            _context = context;
        }

        public async Task<Palestrante[]> GetAllPatestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                                    .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                                .ThenInclude(PE => PE.Evento);
            }

            query = query.OrderBy(p => p.Id)
                            .Where(p => p.Nome.ToLower()
                                        .Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                                    .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                                .ThenInclude(PE => PE.Evento);
            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetAllPatestranteByIdAsync(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                                    .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                                .ThenInclude(PE => PE.Evento);
            }

            query = query.OrderBy(p => p.Id)
                            .Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}