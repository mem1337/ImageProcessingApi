using ImageProcessingService.Models.ImageModels;

namespace ImageProcessingService.Misc;

public interface ITransform
{
    public Task Resize(string imageLocation, int width, int height);
    public Task Crop(string imageLocation, int width, int height);
    public Task Rotate(string imageLocation, int rotations);
    public Task Format(string imageLocation, ImageFormat format);
    public Task Filter(string imageLocation, bool grayscale, bool sepia);
}