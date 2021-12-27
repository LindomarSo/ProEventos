using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProEventos.API.Helpers
{
    public interface IUtils
    {
        Task<string> SaveImage(IFormFile file, string destino);
        void DeleteImage(string imageName, string destino);
    }
}
