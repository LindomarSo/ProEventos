using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/lote")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;

        public LotesController(ILoteService loteService)
        {
            _loteService = loteService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var eventos = await _loteService.GetAllLotesByEventoIdAsync(eventoId);

                if(eventos == null)
                {
                    return NoContent();
                }

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar recuperar o lote {ex.Message}" 
                );
            }
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, [FromBody] LoteDto[] lote)
        {
            try
            {
                var lotes = await _loteService.SaveLote(eventoId, lote);

                if(lotes == null)
                {
                    return NoContent();
                }

                return Ok(lotes);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, 
                    $"Erro ao tentar atualizar. {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                return await _loteService.DeleteLote(loteId, eventoId) ? Ok(new {message = "Deletado"}) : 
                                BadRequest("O Lote não pode ser deletado!");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, 
                    $"Erro ao deletar o lote. {ex.Message}");
            }
        }
    }
}
