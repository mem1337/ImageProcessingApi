namespace ImageProcessingService.Misc;

public interface IHash
{
    public Task<string> GenerateSalt();
    public Task<string> GenerateHash(string password,string salt);
}