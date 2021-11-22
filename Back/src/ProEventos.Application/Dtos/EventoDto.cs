using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório.")]
        // [MinLength(3, ErrorMessage ="O campo {0} Deve conter no mínimo 3 caracteres")]
        // [MaxLength(30, ErrorMessage ="O campo {0} Deve conter no máximo 30 caracteres")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O tema deve ter mínimo 3 e no máximo 30 caracteres" )]
        public string Tema { get; set; }

        [Display(Name = "Qtd Pessoas")]
        [Range(1, 12000, ErrorMessage = "{0} não pode ser menor que 1 ou maior que 12000")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(jpg|jpe?g|png|gif|bmp)$", ErrorMessage = "Não é uma imagem válida (jpg|jpe?g|png|gif|bmp)")]
        public string ImagemURL { get; set; }

        [Display(Name = "telefone")]
        [Phone(ErrorMessage = "O número de {0} não é válido.")]
        public string Telefone { get; set; }

        [Display(Name = "e-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage ="O campo {0} é obrigatório um e-mail válido.")]
        public string Email { get; set; }
        public int UserId { get; set; }
        public UserDto UserDto { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> PalestrantesEventos { get; set; }
    }
}