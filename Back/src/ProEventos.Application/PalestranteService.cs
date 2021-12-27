using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contratos;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersistence _palestrantes;
        private readonly IMapper _mapper;

        public PalestranteService(
            IPalestrantePersistence proPalestrantes, 
            IMapper mapper)
        {
            _palestrantes = proPalestrantes;
            _mapper = mapper;
        }
        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantes.Add<Palestrante>(palestrante);

                if(await _palestrantes.SaveChangesAsync())
                {
                    return _mapper.Map<PalestranteDto>(await _palestrantes.GetPalestranteByUserIdAsync(userId, false));
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var evento = await _palestrantes.GetPalestranteByUserIdAsync(userId, false);

                if(evento == null) 
                    return null; 

                model.Id = evento.Id;
                model.UserId = userId;

                // Mapeando de um objeto para outro objeto
                _palestrantes.Update(_mapper.Map(model, evento));

                if(await _palestrantes.SaveChangesAsync())
                {   
                    // Mapeando de um tipo para outro tipo
                    return _mapper.Map<PalestranteDto>(await _palestrantes.GetPalestranteByUserIdAsync(userId, false));
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetAllPalestranteByUserIdAsync(int userId, bool includePalestrantes)
        {
            try
            {
                var evento =  await _palestrantes.GetPalestranteByUserIdAsync(userId, false);

                if(evento == null)
                {
                    return null;
                }

                return _mapper.Map<PalestranteDto>(evento);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestranteAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var eventos = await _palestrantes.GetAllPalestrantesAsync(pageParams, includeEventos);

                if(eventos == null)
                {
                    return null;
                }

                var retorno = _mapper.Map<PageList<PalestranteDto>>(eventos);

                retorno.CurrentPage = eventos.CurrentPage;
                retorno.TotalPages = eventos.TotalPages;
                retorno.PageSize = eventos.PageSize;
                retorno.TotalCount = eventos.TotalCount;

                return retorno;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}