using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;

        public PalestranteController(IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
        }

        /// <summary>
        /// Método responsável por retornar todos os palestrantes 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams)
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestranteAsync(pageParams, false);

                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);    
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por retornar palestrantes pelo id do usuário 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpGet()]
        public async Task<IActionResult> GetByUserId()
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrantes == null) return NoContent();

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar o palestrante. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por Adicionar um palestrante 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto addDto)
        {
            try
            {
                var palestrante = await _palestranteService.GetAllPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null)
                    palestrante = await _palestranteService.AddPalestrante(User.GetUserId(), addDto);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar o palestrante. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método responsável por atualizar um palestrante 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>PalestranteDto[]</returns>
        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto addDto)
        {
            try
            {
                var palestrantes = await _palestranteService.UpdatePalestrante(User.GetUserId(), addDto);

                if (palestrantes == null) return NoContent();

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar o palestrante. Erro: {ex.Message}");
            }
        }
    }
}
