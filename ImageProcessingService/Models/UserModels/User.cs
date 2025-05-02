using System.ComponentModel.DataAnnotations.Schema;
using ImageProcessingService.Models.ImageModels;
namespace ImageProcessingService.Models.UserModels;

[Table("user")]
public class User
{
    public int UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserHash { get; set; }
    public string UserSalt { get; set; }
    public List<Image> UserImages { get; set; }
}