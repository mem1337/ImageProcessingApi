using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ImageProcessingService.Misc;

public class JWT : IJWT
{
    public Task<string> GenerateToken()
    {
        var key = new SymmetricSecurityKey("31019b7c9129843a3ce838bfd17202d7a5839a59f259a92d9d9523c4a75296f405c2e6d6d62b31ea87bebdf0feac0686dbc812c20619751b901b016d88a7c9e1b9890eccd645f55e0f2313998dcaca51046174fb88e2405377536f0f31f836d254d89e42e2aefff1ba04e70bce4b94a7f769573f9ec995e899a998679d9320bcb12c21a6fea766288c58527df58db02049f56b77fe08277a12789c238358f0fe506ee4895c10aae72f9d16030a9197384ecd60ea749d3b8106c8ba82f2254179225e0a35f30b425036edf25a092fbb465618cb1b61186ecadd9a331a9664d879df338579f675eb617e79b6d64407a43ca452e2341503568a042cc860d396b137"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://127.0.0.1:7088",
            audience: "https://127.0.0.1:7088",
            claims: new List<Claim>(),
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}