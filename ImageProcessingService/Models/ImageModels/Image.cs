using System.ComponentModel.DataAnnotations.Schema;
using ImageProcessingService.Models.UserModels;

namespace ImageProcessingService.Models.ImageModels;
[Table("image")]
public class Image
{
    public int ImageId { get; set; }
    public byte[] ImageBytes { get; set; }
    public string ImageExtension { get; set; }
    public string ImageSize { get; set; }
    public int ImageUserId { get; set; }
    public User User { get; set; }
}