using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class RedeSocialPersistence : GeralPersistence, IRedeSocialPersistence
    {
        private readonly EntityContext _context;

        public RedeSocialPersistence(EntityContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RedeSocial[]> GetAllByEventoIdsAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais.Where(re => re.EventoId == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdsAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
                                                                .Where(rs => rs.PalestranteId == palestranteId);

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial> GetRedeSocialByEventoIdsAsync(int eventoId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking();   

            return await query.FirstOrDefaultAsync(re => re.EventoId == eventoId && re.Id == id);
        }

        public async Task<RedeSocial> GetRedeSocialByPalestranteIdsAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais;

            return await query.FirstOrDefaultAsync(re => re.PalestranteId == palestranteId && re.Id == id);
        }
    }
}
