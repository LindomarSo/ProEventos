using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using System;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocial;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService redeSocial, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _redeSocial = redeSocial;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        /// <summary>
        /// Método responsável por retornar todas as redes socias pelo evento  
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetAllByEventoId(int eventoId)
        {
            try
            {
                if(await AuthorEvento(eventoId))
                {
                    var redesSociais = await _redeSocial.GetAllByEventoIdAsync(eventoId);

                    if (redesSociais == null) return NoContent();
                    
                    return Ok(redesSociais);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar as redes sociais de um evento. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por retornar todas as redes socais pelo palestrante
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpGet("palestrante")]
        public async Task<IActionResult> GetAllByPalestranteId()
        {
            try
            {
                var palestrante = await _palestranteService.GetAllPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null) return NoContent();

                var redesSociais = await _redeSocial.GetAllByPalestranteIdAsync(palestrante.Id);

                return Ok(redesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar todas as redes sociais de um palestrante. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por salvar redes socias de um evento
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveRedesSociaisEvento(int eventoId, [FromBody] RedeSocialDto[] redeSocial)
        {
            try
            {
                if(!await this.AuthorEvento(eventoId)) return Unauthorized();

                var redesSociais = await _redeSocial.SaveByEventoAsync(eventoId, redeSocial);

                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar rede social de um palestrante. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por salvar redes socias de um palestrante
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveRedesSociaisPalestrante([FromBody] RedeSocialDto[] redeSocial)
        {
            try
            {
                var palestrante =  await _palestranteService.GetAllPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null) return Unauthorized();

                var redesSociais = await _redeSocial.SaveByPalestranteAsync(palestrante.Id, redeSocial);

                if(redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar rede social de um palestrante. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por salvar redes socias 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if(!await AuthorEvento(eventoId)) return Unauthorized();

                var redeSocial = await _redeSocial.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocial.DeleteByEventoAsync(eventoId, redeSocialId) ? NoContent() : throw new Exception("Erro ao deletar a rede Social"); 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar a rede social. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por salvar redes socias 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetAllPalestranteByUserIdAsync(User.GetUserId(), false);

                if(palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocial.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocial.DeleteByPalestranteAsync(palestrante.Id, redeSocialId) ? NoContent() : throw new Exception("Erro ao deletar a rede Social do evento");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar a rede social do palestrante. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AuthorEvento(int eventoId)
        {
            var evento = await _eventoService.GetAllEventoByIdAsync(User.GetUserId(), eventoId, false);

            if (evento == null) return false;

            return true;
        }
    }
}
