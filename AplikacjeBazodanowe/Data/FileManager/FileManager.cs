using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using PhotoSauce.MagicScaler;

namespace AplikacjeBazodanowe.Data.FileManager;

public class FileManager : IFileManager
{
    private string _imagePath;
    private string _defaultImageName;

    public string DefaultImage
    {
        get => _defaultImageName;
    }

    public FileManager(IConfiguration config)
    {
        _imagePath = config["Path:Images"];
        _defaultImageName = config["Path:DefaultImageName"];
    }

    public FileStream ImageStream(string imageName)
    {
        return new FileStream(Path.Combine(_imagePath, imageName), FileMode.Open, FileAccess.Read);
    }

    public bool RemoveImage(string image)
    {
        try
        { 
            var file = Path.Combine(_imagePath, image);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public Task<string> SaveImageAsync(IFormFile image)
    {
        try
        {
            var savePath = Path.Combine(_imagePath);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
            var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";

            using (var fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
            {
                MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageOptions());
            }


            return Task.FromResult(fileName);
        } catch (Exception e)
        {
            Console.WriteLine(e.Message); ;
            return Task.FromResult("Error");
        }
    }

    private ProcessImageSettings ImageOptions() => new ProcessImageSettings {
        Width = 800,
        Height = 500,
        ResizeMode = CropScaleMode.Crop,
        EncoderOptions = new JpegEncoderOptions(100, ChromaSubsampleMode.Subsample420, false)
    };
}
