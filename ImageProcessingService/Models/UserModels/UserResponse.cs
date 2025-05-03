namespace ImageProcessingService.Models.UserModels;

public class UserResponse
{
    public bool AuthResult { get; set; }
    public string AuthToken { get; set; }
    public DateTime ExpireDate { get; set; }
}