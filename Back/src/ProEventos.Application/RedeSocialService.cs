using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersistence _redeSocial;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialPersistence redeSocial, IMapper mapper)
        {
            _redeSocial = redeSocial;
            _mapper = mapper;
        }

        public async Task<RedeSocialDto[]> SaveByEventoAsync(int eventoId, RedeSocialDto[] dto)
        {
            try
            {
                var redeSocials = await _redeSocial.GetAllByEventoIdsAsync(eventoId);

                foreach(var model in dto)
                {
                    if(model.Id == 0)
                    {
                        await this.AddRedeSocialAsync(model, eventoId, true);
                    }
                    else
                    {
                        var redeSocial = redeSocials.FirstOrDefault(x => x.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, redeSocial);
                         
                        _redeSocial.Update(redeSocial);
                        await _redeSocial.SaveChangesAsync();
                    }

                }

                return _mapper.Map<RedeSocialDto[]>(await _redeSocial.GetAllByEventoIdsAsync(eventoId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByPalestranteAsync(int palestranteId, RedeSocialDto[] dto)
        {
            try
            {
                var redes = await _redeSocial.GetAllByPalestranteIdsAsync(palestranteId);

                if (redes == null) return null;

                foreach(var model in dto)
                {
                    if(model.Id == 0)
                    {
                        await this.AddRedeSocialAsync(model, palestranteId, false);
                    }
                    else
                    {
                        var redeSocial = redes.FirstOrDefault(rs => rs.Id == model.Id);
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, redeSocial);

                        _redeSocial.Update(redeSocial);
                        await _redeSocial.SaveChangesAsync();
                    }
                }

                return _mapper.Map<RedeSocialDto[]>(await _redeSocial.GetAllByPalestranteIdsAsync(palestranteId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEventoAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var rede = await _redeSocial.GetRedeSocialByEventoIdsAsync(eventoId, redeSocialId);

                if(rede == null) throw new Exception("Rede Social para palestrante não encontrada");

                _redeSocial.Delete(rede);

                return await _redeSocial.SaveChangesAsync() ? true : false; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestranteAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var rede = await _redeSocial.GetRedeSocialByPalestranteIdsAsync(palestranteId, redeSocialId);

                if (rede == null) throw new Exception("Rede Social para evento não encontrada");

                _redeSocial.Delete(rede);

                return await _redeSocial.SaveChangesAsync() ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redes = await _redeSocial.GetAllByEventoIdsAsync(eventoId);

                if(redes == null)
                {
                    return null;
                }

                return _mapper.Map<RedeSocialDto[]>(redes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redes = await _redeSocial.GetAllByPalestranteIdsAsync(palestranteId);

                if (redes == null)
                {
                    return null;
                }

                return _mapper.Map<RedeSocialDto[]>(redes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var rede = await _redeSocial.GetRedeSocialByEventoIdsAsync(eventoId, redeSocialId);

                if (rede == null)
                {
                    return null;
                }

                return _mapper.Map<RedeSocialDto>(rede);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var rede = await _redeSocial.GetRedeSocialByPalestranteIdsAsync(palestranteId, redeSocialId);

                if (rede == null)
                {
                    return null;
                }

                return _mapper.Map<RedeSocialDto>(rede);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddRedeSocialAsync(RedeSocialDto redeSocialDto, int id,  bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(redeSocialDto);

                if (isEvento)
                {
                    redeSocial.EventoId = id;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.PalestranteId = id;
                    redeSocial.Evento = null;
                }

                _redeSocial.Add(redeSocial);
               
                await _redeSocial.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
