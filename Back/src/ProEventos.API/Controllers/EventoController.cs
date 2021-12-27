using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Domain.Models;
using ProEventos.API.Helpers;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/evento")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IAccountService _accountService;
        private readonly IUtils _utils;

        public EventoController(IEventoService eventoService, 
                                IAccountService accountService,
                                IUtils utils)
        {
            _eventoService = eventoService;
            _accountService = accountService;
            _utils = utils;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, false);

                if(eventos == null)
                {
                    return NoContent();
                }

                // var retorno = new List<EventoDto>();

                // foreach(var evento in eventos)
                // {
                //     retorno.Add(new EventoDto(){
                //         Id = evento.Id,
                //         Local = evento.Local,
                //         DataEvento = evento.DataEvento.ToString(),
                //         Tema = evento.Tema,
                //         QtdPessoas = evento.QtdPessoas,
                //         ImagemURL = evento.ImagemURL,
                //         Telefone = evento.Telefone,
                //         Email = evento.Email
                //     });
                // }

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);
                
                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                                    $"Erro ao tentar recuperar eventos. {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventoByIdAsync(User.GetUserId(), id, true);

                if(eventos == null)
                {
                    return NoContent();
                }

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar recuperar o evento {ex.Message}" 
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventoDto evento)
        {
            try
            {
                var eventos = await _eventoService.AddEventos(User.GetUserId(), evento);

                if(eventos == null)
                {
                    return NoContent();
                }

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, 
                    $"Erro ao tentar adicionar. {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventoByIdAsync(User.GetUserId(), eventoId, false);

                if(eventos == null)
                {
                    return NoContent();
                }

                var file = Request.Form.Files[0]; // Pega o primeiro file que tive 
                if(file.Length > 0)
                {
                    if(eventos.ImagemURL != null)
                        _utils.DeleteImage(eventos.ImagemURL, "Images");
                        
                    eventos.ImagemURL = await _utils.SaveImage(file, "Images");
                }

                var retornoImage = await _eventoService.Update(User.GetUserId(), eventoId, eventos);

                return Ok(retornoImage);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, 
                    $"Erro ao tentar adicionar. {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EventoDto evento)
        {
            try
            {
                var eventos = await _eventoService.Update(User.GetUserId(), id, evento);

                if(eventos == null)
                {
                    return NoContent();
                }

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, 
                    $"Erro ao tentar atualizar. {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetAllEventoByIdAsync(User.GetUserId(), id, false);

                if(await _eventoService.DeleteEvento(User.GetUserId(), id)) 
                {
                    _utils.DeleteImage(evento.ImagemURL, "Images");
                    return Ok(new {message = "Deletado"});
                } 
                
                return BadRequest("O evento não pode ser deletado!");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, 
                    $"Erro ao deletar o evento. {ex.Message}");
            }
        }
    }
}
