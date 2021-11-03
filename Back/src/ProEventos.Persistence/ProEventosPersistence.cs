using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class ProEventosPersistence : IProEventosPersistence
    {
        private readonly EntityContext _context;
        public ProEventosPersistence(EntityContext context)
        {
            _context = context;
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(R => R.RedesSociais)
                                                        .Include(L => L.Lotes);

            if(includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos)
                              .ThenInclude(PE => PE.Palestrante);

            query =  query.OrderBy(e => e.Id)
                            .Where(e => e.Tema.ToLower()
                            .Contains(tema.ToLower())); 

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                                .Include(e => e.Lotes)
                                                .Include(e => e.RedesSociais);
                                        
            if(includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos)
                                .ThenInclude(PE => PE.Palestrante);

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }
        
        public async Task<Evento> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                                .Include(e => e.Lotes)
                                                .Include(e => e.RedesSociais);

            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos)
                                .ThenInclude(PE => PE.Palestrante);

            query = query.AsNoTracking()
                         .OrderBy(e => e.Id)
                         .Where(e => e.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}