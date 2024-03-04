using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NTierArchitecture.Business.Services;
public sealed class JwtProvider : IJwtProvider
{
    public string CreateToken()
    {
        JwtSecurityToken jwtSecurityToken = new(
                       issuer: "Cagla Tunc Savas",
                       audience: "School Application",
                       claims: null,
                       notBefore: DateTime.Now,
                       expires: DateTime.Now.AddMinutes(30),
                       signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key my secret key my secret key 1234...my secret key my secret key my secret key 1234...")), SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();
        string token = handler.WriteToken(jwtSecurityToken);

        return token;
    }
}
