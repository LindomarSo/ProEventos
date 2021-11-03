using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class LotePersistence : ILotePersistence
    {
        private readonly EntityContext _context;
        public LotePersistence(EntityContext context)
        {
            _context = context;
        }
        public async Task<Lote[]> GetAllLotessByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query =  query.OrderBy(e => e.Id)
                            .Where(l => l.EventoId == eventoId); 

            return await query.ToArrayAsync();
        }
        
        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                         .OrderBy(lote => lote.Id)
                         .Where(lote => lote.EventoId == eventoId && lote.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}