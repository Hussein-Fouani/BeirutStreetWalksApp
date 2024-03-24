using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        public string CreateToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtoken = new JwtSecurityToken(
                               issuer: configuration["Jwt:Issuer"],
                                              audience: configuration["Jwt:Audience"],
                                                             claims: claims,
                                                                            expires: DateTime.Now.AddMinutes(30),
                                                                                           signingCredentials: creds
                                                                                                          );
            return new JwtSecurityTokenHandler().WriteToken(jwtoken);

        }

    }
}


