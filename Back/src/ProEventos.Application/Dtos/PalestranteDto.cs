using System;
using System.Collections.Generic;

namespace ProEventos.Application.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        public string MiniCurriculo { get; set; }
        public int UserId { get; set; }
        public UserUpdateDto User { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<EventoDto> Palestrantes { get; set; }

        public static implicit operator PalestranteDto(PalestranteAddDto v)
        {
            throw new NotImplementedException();
        }
    }
}