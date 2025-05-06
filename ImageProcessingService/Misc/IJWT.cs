namespace ImageProcessingService.Misc;

public interface IJWT
{
    public Task<string> GenerateToken();
}