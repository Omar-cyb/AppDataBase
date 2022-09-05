using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace AplikacjeBazodanowe.Data.FileManager;
public interface IFileManager
{
    string DefaultImage { get; }
    FileStream ImageStream(string imageName);
    Task<string> SaveImageAsync(IFormFile image);
    bool RemoveImage(string image);
}
