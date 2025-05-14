using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ImageProcessingService.Misc;

public class JWT : IJWT
{
    public Task<string> GenerateToken(int userId)
    {
        var key = new SymmetricSecurityKey("6291e317333c686986923f74ab8874b19d97638e57f2b09d6323c8a365d85817d01794223fd17bfcccd466a99d37cd8b7e76bc1fba4ae4a3a8f5c9fdb6a377145d4c197bb0a771217caa5645cfdf7a626ca6a2ad733e3a6e9c3cc813af9cf9f60f7cae9fef1e9725a6f6d0799605bdbca3af4e837b41111048399e46545c947ec3e702c77b8b0a222b24f4b4a6db2bf4fbb4369d801da58abd14e8e8a294ae16d6a569d1d25e29a02c7799e5d395c8daced2aaea0d8fb6eb3059c4f279a3d6bacb3d1a299d7743b062288e2498e7a3e0aa0f5c95b2287af15c39ce24028f759cfd67a6a474bc4a5856282cb5d47cc659b57ff4b6e2b4a679715db777c4f38f3d"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: "https://localhost:7088",
            audience: "https://localhost:7088",
            claims: new List<Claim> { new ("user_id", userId.ToString()) },
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}