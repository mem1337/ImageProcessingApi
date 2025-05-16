namespace ImageProcessingService.Models.ImageModels;

public class Format
{
    public ImageFormat ImageFormat { get; set; } = 0;
}

public enum ImageFormat
{
    png=1,
    bmp=2,
    jpeg=3
}