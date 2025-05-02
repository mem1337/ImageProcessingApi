using System.Security.Cryptography;
using System.Text;

namespace ImageProcessingService.Misc;

public static class Hash
{
    private const int SizeInBytes = 128 / 8;

    public static Task<string> GenerateSalt()
    {
        byte[] saltBytes = new byte[SizeInBytes];
        {
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
        }
        return Task.FromResult(Convert.ToBase64String(saltBytes));
    }

    public static Task<string> GenerateHash(string password, string salt)
    {
        string hashedPassword;
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(password + salt);
            var hashBytes = md5.ComputeHash(inputBytes);
            hashedPassword = Convert.ToHexString(hashBytes);
        }
        return Task.FromResult(hashedPassword);
    }
}