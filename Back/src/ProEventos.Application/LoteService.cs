using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Contratos;
using ProEventos.Persistence.Contratos;
using System.Linq;
using ProEventos.Domain.Models;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly ILotePersistence _Lote;
        private readonly IGeralPersistence _geralPersistence;
        private readonly IMapper _mapper;

        public LoteService(
            ILotePersistence Lote, 
            IGeralPersistence geralPersistence,
            IMapper mapper)
        {
            _Lote = Lote;
            _geralPersistence = geralPersistence;
            _mapper = mapper;
        }

        private async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersistence.Add(lote);

                await _geralPersistence.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _Lote.GetAllLotessByEventoIdAsync(eventoId);

                if(lotes == null) 
                    return null; 
                
                foreach(var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;

                        var map = _mapper.Map(model, lote);

                        _geralPersistence.Update(map);
                        await _geralPersistence.SaveChangesAsync();
                    }
                }

                return _mapper.Map<LoteDto[]>(await _Lote.GetAllLotessByEventoIdAsync(eventoId));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int loteId, int eventoId)
        {
            try
            {
                var lote = await _Lote.GetLoteByIdsAsync(eventoId, loteId);

                if(lote == null)
                {
                   throw new Exception("Evento não encontrado");
                }

                _geralPersistence.Delete(lote);

                return await _geralPersistence.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int loteId, int eventoId)
        {
            try
            {
                var lote =  await _Lote.GetLoteByIdsAsync(eventoId, loteId);

                if(lote == null)
                {
                    return null; 
                }

                return _mapper.Map<LoteDto>(lote);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetAllLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await  _Lote.GetAllLotessByEventoIdAsync(eventoId);

                if(lotes == null)
                {
                    return null;
                }

                var retorno = _mapper.Map<LoteDto[]>(lotes);

                return retorno;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}