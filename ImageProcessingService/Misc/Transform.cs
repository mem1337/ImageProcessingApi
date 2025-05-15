using ImageProcessingService.Models.ImageModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace ImageProcessingService.Misc;


public class Transform : ITransform
{
    public async Task Resize(string imageLocation, int width, int height)
    {
        using (Image image = Image.Load(imageLocation))
        {
            image.Mutate(x => x.Resize(width,height));
            image.Save(imageLocation);
        }
    }

    public async Task Crop(string imageLocation, int width, int height)
    {
        using (Image image = Image.Load(imageLocation))
        {
            image.Mutate(x => x.Crop(width,height));
            image.Save(imageLocation);
        }
    }

    public async Task Rotate(string imageLocation, int rotations)
    {
        using (Image image = Image.Load(imageLocation))
        {
            var degrees = 90 * rotations;
            image.Mutate(x => x.Rotate(degrees));
            image.Save(imageLocation);
        }
    }

    public async Task Format(string imageLocation, ImageFormat format)
    {
        using (Image image = Image.Load(imageLocation))
        {
        }
    }

    public async Task Filter(string imageLocation, bool grayscale, bool sepia)
    {
        using (Image image = Image.Load(imageLocation))
        {
            if (grayscale) image.Mutate(x=>x.Grayscale());
            
            if (sepia) image.Mutate(x=>x.Sepia());
            
            image.Save(imageLocation);
        }
    }

}