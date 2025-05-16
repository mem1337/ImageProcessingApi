using ImageProcessingService.Models.ImageModels;

namespace ImageProcessingService.Misc;

public interface ITransform
{
    public Task Resize(Image imageFile, int width, int height);
    public Task Crop(Image imageFile, int width, int height);
    public Task Rotate(Image imageFile, int rotations);
    public Task<(string, string)> Format(Image imageFile, ImageFormat format);
    public Task Filter(Image imageFile, bool grayscale, bool sepia);
}