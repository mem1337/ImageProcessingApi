namespace ImageProcessingService.Models;

public class UserJwtResponse
{
    public int UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserJwt { get; set; }
}