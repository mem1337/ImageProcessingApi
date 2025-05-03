namespace ImageProcessingService.Misc;

public interface IJwt
{
    public Task<string> GenerateToken();
}