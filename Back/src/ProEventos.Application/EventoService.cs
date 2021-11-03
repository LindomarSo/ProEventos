using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Contratos;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IProEventosPersistence _proEventos;
        private readonly IGeralPersistence _geralPersistence;
        private readonly IMapper _mapper;

        public EventoService(
            IProEventosPersistence proEventos, 
            IGeralPersistence geralPersistence,
            IMapper mapper)
        {
            _proEventos = proEventos;
            _geralPersistence = geralPersistence;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _geralPersistence.Add<Evento>(evento);

                if(await _geralPersistence.SaveChangesAsync())
                {
                    return _mapper.Map<EventoDto>(await _proEventos.GetAllEventoByIdAsync(evento.Id, false));
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> Update(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _proEventos.GetAllEventoByIdAsync(eventoId, false);

                if(evento == null) 
                    return null; 

                model.Id = evento.Id;

                // Mapeando de um objeto para outro objeto
                _geralPersistence.Update(_mapper.Map(model, evento));

                if(await _geralPersistence.SaveChangesAsync())
                {   
                    // Mapeando de um tipo para outro tipo
                    return _mapper.Map<EventoDto>(await _proEventos.GetAllEventoByIdAsync(model.Id, false));
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var entity = await _proEventos.GetAllEventoByIdAsync(eventoId, false);

                if(entity == null)
                {
                   throw new Exception("Evento não encontrado");
                }

                _geralPersistence.Delete(entity);

                return await _geralPersistence.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            try
            {
                var evento =  await _proEventos.GetAllEventoByIdAsync(eventoId, false);

                if(evento == null)
                {
                    return null;
                }

                return _mapper.Map<EventoDto>(evento);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await  _proEventos.GetAllEventosAsync(includePalestrantes);

                if(eventos == null)
                {
                    return null;
                }

                var retorno = _mapper.Map<EventoDto[]>(eventos);

                return retorno;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos =  await _proEventos.GetAllEventosByTemaAsync(tema,  includePalestrantes);

                if(eventos == null)
                {
                    return null;
                }

                return _mapper.Map<EventoDto[]>(eventos);;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}