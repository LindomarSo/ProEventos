using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersistence
    {
        /// <summary>
        /// Método responsável por retornar todos os lotes de um evento
        /// </summary>
        /// <param name="eventoId"> Código chave da tabela evento </param>
        /// <returns> Lista de Lotes </returns>
         Task<Lote[]> GetAllLotessByEventoIdAsync(int eventoId);
         /// <summary>
         /// Método get que retornará apenas um lote
         /// </summary>
         /// <param name="eventoId"> Código chave da tabela evento</param>
         /// <param name="loteId"> Código chave do meu lote </param>
         /// <returns> Apenas um lote </returns>
         Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}