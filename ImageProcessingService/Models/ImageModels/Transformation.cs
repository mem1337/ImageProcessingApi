namespace ImageProcessingService.Models.ImageModels;

public class Transformation
{
    public Resize Resize { get; set; }
    public Crop Crop { get; set; }
    public Rotate Rotate { get; set; }
    public Format Format { get; set; }
    public Filter Filter { get; set; }
}