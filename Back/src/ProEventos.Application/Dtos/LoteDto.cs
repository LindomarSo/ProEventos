namespace ProEventos.Application.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        public string Nomwe { get; set; }
        public decimal Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public int Quantidade { get; set; }
        public int EventoId { get; set; }
    }
}