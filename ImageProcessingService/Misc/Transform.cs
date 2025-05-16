using System.Reflection;
using ImageProcessingService.Models.ImageModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using ImageSharp = SixLabors.ImageSharp.Image;
using Image = ImageProcessingService.Models.ImageModels.Image;

namespace ImageProcessingService.Misc;


public class Transform : ITransform
{
    public async Task Resize(Image imageFile, int width, int height)
    {
        using (ImageSharp image = await ImageSharp.LoadAsync(imageFile.ImageLocationFull))
        {
            image.Mutate(x => x.Resize(width,height));
            await image.SaveAsync(imageFile.ImageLocationFull);
        }
    }

    public async Task Crop(Image imageFile, int width, int height)
    {
        using (ImageSharp image = await ImageSharp.LoadAsync(imageFile.ImageLocationFull))
        {
            image.Mutate(x => x.Crop(width,height));
            await image.SaveAsync(imageFile.ImageLocationFull);
        }
    }

    public async Task Rotate(Image imageFile, int rotations)
    {
        using (ImageSharp image = await ImageSharp.LoadAsync(imageFile.ImageLocationFull))
        {
            var degrees = 90 * rotations;
            image.Mutate(x => x.Rotate(degrees));
            await image.SaveAsync(imageFile.ImageLocationFull);
        }
    }

    public async Task<(string, string)> Format(Image imageFile, ImageFormat format)
    {
        using (ImageSharp image = await ImageSharp.LoadAsync(imageFile.ImageLocationFull))
        {
            var path = $"{imageFile.ImageLocation}{imageFile.ImageName}";
            switch (format)
            {
                case ImageFormat.png:
                {
                    await image.SaveAsPngAsync($"{path}.png");
                    return (path,".png");
                }
                case ImageFormat.bmp:
                {
                    await image.SaveAsBmpAsync($"{path}.bmp");
                    return (path,".bmp");
                }
                case ImageFormat.jpeg:
                {
                    await image.SaveAsJpegAsync($"{path}.jpeg");
                    return (path,".jpeg");
                }
            }
        }
        return (null,null);
    }

    public async Task Filter(Image imageFile, bool grayscale, bool sepia)
    {
        using (ImageSharp image = await ImageSharp.LoadAsync(imageFile.ImageLocationFull))
        {
            if (grayscale) image.Mutate(x=>x.Grayscale());
            
            if (sepia) image.Mutate(x=>x.Sepia());
            
            await image.SaveAsync(imageFile.ImageLocationFull);
        }
    }

}