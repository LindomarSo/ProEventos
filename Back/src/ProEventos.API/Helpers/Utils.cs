using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.API.Helpers
{
    public class Utils : IUtils
    {
        private readonly IWebHostEnvironment _webHost;

        public Utils(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public void DeleteImage(string imageName, string destino)
        {
            var imagePath = Path.Combine(_webHost.ContentRootPath, $"Resources/{destino}", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        public async Task<string> SaveImage(IFormFile file, string destino)
        {
            var imageName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(10)
                                                                                        .ToArray())
                                                                                        .Replace(" ", "-");
            
            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(file.FileName)}";

            var imagePath = Path.Combine(_webHost.ContentRootPath, $"Resources/{destino}", imageName);

            using(var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
