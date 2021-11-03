namespace ProEventos.Domain.Models
{
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; }
        public int EventoId { get; set; }
        public Palestrante Palestrante { get; set; }
        public Evento Evento { get; set; }
    }
}